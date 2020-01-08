using System; // For IntPtr
using System.Runtime.InteropServices; // DllImport
using System.Diagnostics; // Process

public class wmctrl
{

    // --------------------------------------------------------------------------------
    // --- Switch to Window
    // --------------------------------------------------------------------------------
    //dll import (can't be in method, but needs to be in class
    [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd);

    public static int SwitchToWindow(string procName){
        // Getting window matching 
        Process[] procs = Process.GetProcessesByName(procName);
        int nProcs = procs.Length;
        if (nProcs < 1) {
            Console.WriteLine("Error: No process found for name: {0}", procName);
            return -1 ;
        }else{
            // We'll use the first window we found
            Process proc=procs[0];
            if (nProcs >1) {
                Console.WriteLine("{0} processes found with name: {1}",nProcs,procName);
                Console.WriteLine("Using first process:");
                Console.WriteLine("Process Name: {0} ID: {1} Title: {2}", proc.ProcessName, proc.Id, proc.MainWindowTitle);
            }
            // --- Switching to window using user32.dll function
            SwitchToThisWindow(proc.MainWindowHandle);
            return 0;
        }
    }

    // --------------------------------------------------------------------------------
    // --- Switch to Window (TITLE)
    // --------------------------------------------------------------------------------
    
    public static int SwitchToWindowTitle(string procTitle)
    {
        int ret = -1;
        Process[] procs = Process.GetProcesses();
        foreach (Process proc in procs)
        {
            if (! String.IsNullOrEmpty(proc.MainWindowTitle) && string.Compare(procTitle, proc.MainWindowTitle) == 0)
            {
                Console.WriteLine("Process Name: {0} ID: {1} Title: {2}", proc.ProcessName, proc.Id, proc.MainWindowTitle);
                SwitchToThisWindow(proc.MainWindowHandle); //Just going to use the first window as per usual
                ret = 0;
                break;
            }
        }
        if (ret == -1)
        {
            Console.WriteLine("Error: No process found for title: {0}", procTitle);
        }
        return ret;
    }

    // --------------------------------------------------------------------------------
    // --- Switch to Window (PID)
    // --------------------------------------------------------------------------------

    public static int SwitchToWindowPID(string procPIDstr)
    {
        int ret = -1;
        int procPID;
        if (Int32.TryParse(procPIDstr, out procPID)){
            Process[] procs = Process.GetProcesses();
            foreach (Process proc in procs)
            {
                if (procPID == proc.Id) //Can processes have no PID in MS windows? I have no idea.
                {
                    Console.WriteLine("Process Name: {0} ID: {1} Title: {2}", proc.ProcessName, proc.Id, proc.MainWindowTitle);
                    SwitchToThisWindow(proc.MainWindowHandle); //Just going to use the first window as per usual
                    ret = 0;
                    break;
                }
            }
            if (ret == -1)
            {
                Console.WriteLine("Error: No process found for PID: {0}", procPIDstr);
            }
        }
        else
        {
            Console.WriteLine("Error: Invalid PID: {0}", procPIDstr);
        }
        return ret;
    }

    // --------------------------------------------------------------------------------
    // --- List Windows info
    // --------------------------------------------------------------------------------
    public static int ListWindows(){
        Process[] processlist = Process.GetProcesses();
        Console.WriteLine("ID: \t Name:\t Title:");
        Console.WriteLine("-------------------------------------------------");
        foreach (Process proc in processlist)
        {
            if (!String.IsNullOrEmpty(proc.MainWindowTitle))
            {
                Console.WriteLine("{0}\t {1}\t {2}", proc.Id,proc.ProcessName,  proc.MainWindowTitle);
            }
        }
        return 0;
    }

    // --------------------------------------------------------------------------------
    // --- Print command usage 
    // --------------------------------------------------------------------------------
    public static void print_usage(){
        Console.WriteLine("");
        Console.WriteLine("usage: wmctrl [options] [args]");
        Console.WriteLine("");
        Console.WriteLine("options:");
        Console.WriteLine("  -h          : show this help");
        Console.WriteLine("  -l          : list windows");
        Console.WriteLine("  -a <PNAME>  : switch to the window of the process name <PNAME>");
        Console.WriteLine("  -b <PTITLE> : switch to the window of the process title <PTITLE>");
        Console.WriteLine("  -p <PID>    : switch to the window of the process ID <PID>");
        Console.WriteLine("");
    
    }

    // --------------------------------------------------------------------------------
    // --- Main Program 
    // --------------------------------------------------------------------------------
    public static int Main(string[] args)
    {
        int status=0; // Return status for Main

        // --------------------------------------------------------------------------------
        // --- Parsing arguments 
        // --------------------------------------------------------------------------------
        int nArgs=args.Length;
        if (nArgs==0){
            Console.WriteLine("Error: insufficient command line arguments");
            print_usage();
            return 0;
        }
        int i=0;
        while (i<nArgs) {
            string s=args[i];
            switch(s){
                case "-h": // Help
                    print_usage();
                    i=i+1;
                    break;
                case "-a": // Switch to Window
                    if (i+1<nArgs) {
                        status=SwitchToWindow(args[i+1]);
                        i=i+2;
                    }else{
                        Console.WriteLine("Error: command line option -a needs to be followed by a process name.");
                        status=-1;
                    }
                    break;
                case "-b": // Switch to window (TITLE)
                    if (i+1<nArgs) {
                        status=SwitchToWindowTitle(args[i+1]);
                        i=i+2;
                    }else{
                        Console.WriteLine("Error: command line option -a needs to be followed by a process title.");
                        status=-1;
                    }
                    break;
                case "-p": // Switch to window (PID)
                    if (i+1<nArgs) {
                        status=SwitchToWindowPID(args[i+1]);
                        i=i+2;
                    }else{
                        Console.WriteLine("Error: command line option -a needs to be followed by a process ID.");
                        status=-1;
                    }
                    break;
                case "-l": // List Windows
                    status=ListWindows();
                    i++;
                    break;
                default:
                    Console.WriteLine("Skipped argument: "+ args[i]);
                    i++;
                    break;
            }
            if (status!=0) {
                // If an error occured, print usage and exit
                print_usage();
                return status;
            }
        }
        //
        return status;
    }



}
