using System.Diagnostics;

namespace UnmanagedThreadUtils
{
    internal static class Shell
    {
        /// <summary>
        /// Executes Bash command.
        /// </summary>
        public static string BashExecute(string args)
        {
            return Execute("/bin/bash", args);
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        private static string Execute(string file, string args)
        {
            var escapedArgs = args.Replace("\"", "\\\"");

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = file,
                    Arguments = string.Format("-c \"{0}\"", escapedArgs),
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();

            var res = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return res;
        }
    }
}
