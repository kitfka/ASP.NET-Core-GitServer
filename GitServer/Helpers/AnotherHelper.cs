using GitServer.ApplicationCore.Models;
using GitServer.Extensions;
using GitServer.Models;
using LibGit2Sharp;
using System.Linq;

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
            result = "WTF";
        }

        //repo.

        return result;
    }
}
