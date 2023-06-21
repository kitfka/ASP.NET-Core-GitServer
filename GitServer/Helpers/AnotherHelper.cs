using GitServer.ApplicationCore.Models;
using GitServer.Extensions;
using GitServer.Models;
using LibGit2Sharp;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    public static string Spike(LibGit2Sharp.Repository repo, FileViewModel fvm)
    {
        return Spike2(repo, fvm);
        //repo.
        //var result = repo.Lookup<Commit>(fvm.Object.Sha);
        //var result = repo.Lookup<Commit>(fvm.Object.Sha);

        string result = "";


        // THIS IS BAD DUM AND SLOW i think

        foreach (var commit in repo.Commits)
        {
            foreach (var item in commit.Tree)
            {
                if (item.Target == fvm.Object)
                {
                    result = commit.Message;
                    break;
                }
            }
        }
        if (string.IsNullOrEmpty(result))
        {
            var x = repo.Lookup("main:cs unit tests/ConsoleApp/ConsoleApp/ConsoleApp");
            // I GAVE UP< BING GOOD LUCK WITH THIS ONE!

            //var commit = repo.Lookup<LibGit2Sharp.Commit>(fvm.SHA1);
            var commit = repo.Lookup(fvm.SHA1);
            
            result = "WTF";

        }

        //repo.

        return result;
    }

    // Yess a even more jank solution. I want something that works!
    public static string Spike2(LibGit2Sharp.Repository repo, FileViewModel fvm)
    {
        /// I know this works. but there is a problem
        /// I had to hardcode the branch that is used.
        /// You don't want this, for now it wil be fine. But what if we want to see the tree from a specific commit?
        /// We could do this, and traverse the commits using parants? well that might also cause problems later on. But would be better then this!

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
                break;
            }
        }
        
        if (c != null)
        {
            return c.Message;
        }
        return "N/A";
    }

    public static string GetTimeStamp(LibGit2Sharp.Repository repo, FileViewModel fvm)
    {
        // check Spike2 for the problems!
        return GetSpikeCommit(repo, fvm).Committer.When.ToString("yyyy-MM-dd HH:mm:ss");
    }

    private static Commit GetSpikeCommit(LibGit2Sharp.Repository repo, FileViewModel fvm)
    {
        // check Spike2 for the problems!

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
                break;
            }
        }

        if (c != null)
        {
            return c;
        }
        return null;
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
