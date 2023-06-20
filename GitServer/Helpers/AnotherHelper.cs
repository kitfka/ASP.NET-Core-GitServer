using GitServer.ApplicationCore.Models;
using GitServer.Extensions;
using GitServer.Models;
using LibGit2Sharp;
using System.Linq;

namespace GitServer.Helpers;

public static class AnotherHelper
{
    public static string Spike(LibGit2Sharp.Repository repo, FileViewModel fvm)
    {

        //repo.
        //var result = repo.Lookup<Commit>(fvm.Object.Sha);
        //var result = repo.Lookup<Commit>(fvm.Object.Sha);

        string result = "";

        foreach (var commit in repo.Commits)
        {
            if (commit.Id == fvm.Object.Id)
            {
                result = commit.Message;
            }
        }
        //repo.Commits.TraverseTree(
        //    x => x.Where(
        //        y => y.Tree.Where(
        //            z => z.Target.Id == fvm.Object.Id
        //            ).FirstOrDefault()
        //        ).FirstOrDefault()
        //    );

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
