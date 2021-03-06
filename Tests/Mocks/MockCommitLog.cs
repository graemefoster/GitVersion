using System.Collections;
using System.Collections.Generic;
using LibGit2Sharp;

public class MockCommitLog:ICommitLog,ICollection<Commit>
{
    public List<Commit> Commits = new List<Commit>();

    public IEnumerator<Commit> GetEnumerator()
    {
        return Commits.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public CommitSortStrategies SortedBy { get; private set; }
    public void Add(Commit item)
    {
        Commits.Add(item);
    }

    public void Clear()
    {
        Commits.Clear();
    }
    

    public bool Contains(Commit item)
    {
        return Commits.Contains(item);
    }

    public void CopyTo(Commit[] array, int arrayIndex)
    {
        Commits.CopyTo(array, arrayIndex);
    }

    public bool Remove(Commit item)
    {
        return Commits.Remove(item);
    }

    public int Count { get { return Commits.Count; } }
    public bool IsReadOnly {get { return false; } 
    }
}