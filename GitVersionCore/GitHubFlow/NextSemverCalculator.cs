﻿namespace GitVersion
{
    using System.Linq;

    public class NextSemverCalculator
    {
        NextVersionTxtFileFinder nextVersionTxtFileFinder;
        LastTaggedReleaseFinder lastTaggedReleaseFinder;
        OtherBranchVersionFinder unknownBranchFinder;
        GitVersionContext context;
        MergedBranchesWithVersionFinder mergedBranchesWithVersionFinder;

        public NextSemverCalculator(
            NextVersionTxtFileFinder nextVersionTxtFileFinder,
            LastTaggedReleaseFinder lastTaggedReleaseFinder,
            GitVersionContext context)
        {
            this.nextVersionTxtFileFinder = nextVersionTxtFileFinder;
            this.lastTaggedReleaseFinder = lastTaggedReleaseFinder;
            mergedBranchesWithVersionFinder = new MergedBranchesWithVersionFinder(context);
            unknownBranchFinder = new OtherBranchVersionFinder();
            this.context = context;
        }

        public SemanticVersion NextVersion()
        {
            var versionZero = new SemanticVersion();
            var lastRelease = lastTaggedReleaseFinder.GetVersion().SemVer;
            var fileVersion = nextVersionTxtFileFinder.GetNextVersion();
            var mergedBranchVersion = mergedBranchesWithVersionFinder.GetVersion();
            var otherBranchVersion = unknownBranchFinder.FindVersion(context);
            if (otherBranchVersion != null && otherBranchVersion.PreReleaseTag != null && otherBranchVersion.PreReleaseTag.Name == "release")
                otherBranchVersion.PreReleaseTag.Name = "beta";

            var maxCalculated = new[]{ fileVersion, otherBranchVersion, mergedBranchVersion }.Max();

            if (lastRelease == versionZero && maxCalculated == versionZero)
            {
                return new SemanticVersion
                {
                    Minor = 1
                };
            }

            if (maxCalculated <= lastRelease)
            {
                return new SemanticVersion
                {
                    Major = lastRelease.Major,
                    Minor = lastRelease.Minor,
                    Patch = lastRelease.Patch + 1
                };
            }

            return maxCalculated;
        }
    }
}