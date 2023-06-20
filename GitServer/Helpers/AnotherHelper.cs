using GitServer.ApplicationCore.Models;
using GitServer.Extensions;
using GitServer.Models;
using LibGit2Sharp;

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


        return result;
    }
}
