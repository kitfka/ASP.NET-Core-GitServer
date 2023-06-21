using GitServer.Models;
using LibGit2Sharp;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace GitServer.Helpers;

public static class AnotherHelper
{

    /// <summary>
    /// A temp method to retrieve last commit message of a FileViewModel.
    /// Now we will need to retrieve the other one!
    /// Also might not work when not in the first tree thingy!
    /// </summary>
    /// <param name="repo"></param>
    /// <param name="fvm"></param>
    /// <returns></returns>
    public static string GetMessage(LibGit2Sharp.Repository repo, FileViewModel fvm)
    {
        Commit c = GetSpikeCommit(repo, fvm);
        return c == null ? "N/A" : c.Message;
    }

    public static string GetTimeStamp(LibGit2Sharp.Repository repo, FileViewModel fvm)
    {
        var x = GetSpikeCommit(repo, fvm);
        return x == null ? "N/A" : x.Committer.When.ToString("yyyy-MM-dd HH:mm:ss");
    }

    private static Commit GetSpikeCommit(LibGit2Sharp.Repository repo, FileViewModel fvm)
    {
        /// I know this works. but there is a problem
        /// I had to hardcode the branch that is used.
        /// You don't want this, for now it wil be fine. But what if we want to see the tree from a specific commit?
        /// We could do this, and traverse the commits using parants? well that might also cause problems later on. But would be better then this!


        // Yes i checked, this will break when looking in old versions. still kinda works tho, just nog good enough!
        //https://stackoverflow.com/questions/23303549/what-are-commit-ish-and-tree-ish-in-git

        Commit c = null;
        foreach (var commit in repo.Branches["main"].Commits)
        {
            var yy = commit[fvm.Path];

            // this is not really great, but a decent start!
            if (yy == null)
            {
                break;
            }
            if (yy.Target.Id == fvm.Object.Id)
            {
                c = commit;
            }
            else
            {
                if (c == null)
                {
                    continue;
                }
                break;
            }
        }

        if (c != null)
        {
            return c;
        }
        return null;
    }


    public static string GetReadMe(LibGit2Sharp.Repository repo)
    {
        foreach (var item in repo.Branches[Constants.Constants.DefaultBranch].Tip.Tree)
        {
            if (item.TargetType == TreeEntryTargetType.Blob)
            {
                if (ValidReadme(item.Name))
                {
                    return item.Target.Peel<Blob>().GetContentText();
                }
            }
        }
        return "";
    }
    public static bool ContainsReadMe(LibGit2Sharp.Repository repo)
    {
        foreach (var item in repo.Branches[Constants.Constants.DefaultBranch].Tip.Tree)
        {
            if (item.TargetType == TreeEntryTargetType.Blob)
            {
                if (ValidReadme(item.Name))
                {
                    return true;
                }
            }
        }
        return false;
    }

    
    private static bool ValidReadme(string name)
    {
        string nn = name.ToLower();

        return nn is "readme.md" or "readme.txt";
    }

    public static IList<string> SpikePath(string path)
    {
        // We no longer need this XD
        List<string> result = new();

        string[] split = path.Split('/');
        StringBuilder sb = new();

        foreach (string s in split)
        {
            sb.Append(s);
            result.Add(sb.ToString());
            sb.Append('/');
        }

        return result;
    }
}
