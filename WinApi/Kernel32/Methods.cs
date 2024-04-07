using System;
using System.Runtime.InteropServices;
using System.Text;
using NetCoreEx.Geometry;
using WinApi;
using WinApi.Kernel32;

namespace WinApi.Kernel32;

public static class Kernel32Methods
{
  public const string Kernel32LibraryName = "kernel32";
  public const string NtLibraryName = "ntdll";

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern uint GetLastError();

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern void SetLastError(uint dwErrCode);

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern bool DisableThreadLibraryCalls(IntPtr hModule);

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern void Sleep(uint dwMilliseconds);

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern uint GetTickCount();

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern ulong GetTickCount64();

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern bool QueryPerformanceCounter(out long value);

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern bool QueryPerformanceFrequency(out long value);

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern void QueryUnbiasedInterruptTime(out ulong unbiasedTime);

  [DllImport(Kernel32LibraryName)]
  public static extern void GetLocalTime(out SystemTime lpSystemTime);

  [DllImport(Kernel32LibraryName)]
  public static extern bool SetLocalTime(ref SystemTime lpSystemTime);

  [DllImport(Kernel32LibraryName)]
  public static extern void GetSystemTime(out SystemTime lpSystemTime);

  [DllImport(Kernel32LibraryName)]
  public static extern bool SetSystemTime(ref SystemTime lpSystemTime);

  #region Console Functions

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern bool AllocConsole();

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern bool FreeConsole();

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern bool AttachConsole(uint dwProcessId);

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern IntPtr GetConsoleWindow();

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern IntPtr GetStdHandle(uint nStdHandle);

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern bool SetStdHandle(uint nStdHandle, IntPtr hHandle);

  [DllImport(Kernel32LibraryName, CharSet = Properties.BuildCharSet)]
  public static extern bool SetConsoleTitle(string lpConsoleTitle);

  [DllImport(Kernel32LibraryName, CharSet = Properties.BuildCharSet)]
  public static extern uint GetConsoleTitle(StringBuilder lpConsoleTitle, uint nSize);

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern bool SetConsoleWindowInfo(IntPtr hConsoleOutput, int bAbsolute, [In] ref RectangleS lpConsoleWindow);

  #endregion

  #region Memory Methods

  [DllImport(Kernel32LibraryName, ExactSpelling = true, EntryPoint = "RtlZeroMemory")]
  public static extern void ZeroMemory(IntPtr dest, IntPtr size);

  [DllImport(Kernel32LibraryName, ExactSpelling = true, EntryPoint = "RtlSecureZeroMemory")]
  public static extern void SecureZeroMemory(IntPtr dest, IntPtr size);

  #endregion

  #region Handle and Object Functions

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern bool CloseHandle(IntPtr hObject);

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern bool DuplicateHandle(
    IntPtr hSourceProcessHandle, IntPtr hSourceHandle, IntPtr hTargetProcessHandle,
    out IntPtr lpTargetHandle,
    uint dwDesiredAccess,
    int bInheritHandle,
    DuplicateHandleFlags dwOptions);

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern bool GetHandleInformation(IntPtr hObject, out HandleInfoFlags lpdwFlags);

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern bool SetHandleInformation(IntPtr hObject, HandleInfoFlags dwMask, HandleInfoFlags dwFlags);

  #endregion

  #region DLL Methods

  [DllImport(Kernel32LibraryName, CharSet = Properties.BuildCharSet)]
  public static extern IntPtr GetModuleHandle(IntPtr modulePtr);

  [DllImport(Kernel32LibraryName, CharSet = Properties.BuildCharSet)]
  public static extern bool GetModuleHandleEx(GetModuleHandleFlags dwFlags, string lpModuleName,
    out IntPtr phModule);

  [DllImport(Kernel32LibraryName, CharSet = Properties.BuildCharSet)]
  public static extern IntPtr GetModuleHandle(string lpModuleName);

  [DllImport(Kernel32LibraryName, CharSet = Properties.BuildCharSet)]
  public static extern IntPtr LoadLibrary(string fileName);

  [DllImport(Kernel32LibraryName, CharSet = Properties.BuildCharSet)]
  public static extern IntPtr LoadLibraryEx(string fileName, IntPtr hFileReservedAlwaysZero,
    LoadLibraryFlags dwFlags);

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern bool FreeLibrary(IntPtr hModule);

  [DllImport(Kernel32LibraryName, CharSet = CharSet.Ansi)]
  public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

  [DllImport(Kernel32LibraryName, CharSet = Properties.BuildCharSet)]
  public static extern bool SetDllDirectory(string fileName);

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern bool SetDefaultDllDirectories(LibrarySearchFlags directoryFlags);

  [DllImport(Kernel32LibraryName, CharSet = Properties.BuildCharSet)]
  public static extern IntPtr AddDllDirectory(string newDirectory);

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern bool RemoveDllDirectory(IntPtr cookieFromAddDllDirectory);

  #endregion

  #region System Information Functions

  [DllImport(Kernel32LibraryName, CharSet = Properties.BuildCharSet)]
  public static extern uint GetSystemDirectory(StringBuilder lpBuffer, uint uSize);

  [DllImport(Kernel32LibraryName, CharSet = Properties.BuildCharSet)]
  public static extern uint GetWindowsDirectory(StringBuilder lpBuffer, uint uSize);

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern uint GetVersion();

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern bool IsWow64Process(IntPtr hProcess, out int isWow64Process);

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern void GetNativeSystemInfo(out SystemInfo lpSystemInfo);

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern void GetSystemInfo(out SystemInfo lpSystemInfo);

  #endregion

  #region Os Version Functions
  [DllImport(Kernel32LibraryName, ExactSpelling = true, CharSet = CharSet.Unicode)]
  public static extern bool IsWindows7OrGreater();

  [DllImport(Kernel32LibraryName, ExactSpelling = true, CharSet = CharSet.Unicode)]
  public static extern bool IsWindows8OrGreater();

  [DllImport(Kernel32LibraryName, ExactSpelling = true, CharSet = CharSet.Unicode)]
  public static extern bool IsWindows8Point1OrGreater();

  [DllImport(Kernel32LibraryName, ExactSpelling = true, CharSet = CharSet.Unicode)]
  public static extern bool IsWindows10OrGreater();
  #endregion

  #region Process and Thread Functions

  [DllImport(NtLibraryName, ExactSpelling = true)]
  public static extern int RtlGetVersion(ref RTL_OSVERSIONINFOEX lpVersionInformation);

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern uint GetCurrentProcessId();

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern IntPtr GetCurrentProcess();

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern uint GetCurrentThreadId();

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern IntPtr GetCurrentThread();

  [DllImport(Kernel32LibraryName, ExactSpelling = true)]
  public static extern uint GetCurrentProcessorNumber();

  #endregion

  #region File Management Functions

  [DllImport(Kernel32LibraryName, CharSet = Properties.BuildCharSet)]
  public static extern FileAttributes GetFileAttributes(string lpFileName);

  [DllImport(Kernel32LibraryName, CharSet = Properties.BuildCharSet)]
  public static extern bool SetFileAttributes(string lpFileName, FileAttributes dwFileAttributes);

  [DllImport(Kernel32LibraryName, CharSet = Properties.BuildCharSet)]
  public static extern bool GetFileAttributesEx(string lpFileName, FileAttributeInfoLevel fInfoLevelId,
    out FileAttributeData lpFileInformation);

  [DllImport(Kernel32LibraryName, CharSet = Properties.BuildCharSet)]
  public static extern IntPtr CreateFile(string lpFileName,
    uint dwDesiredAccess,
    FileShareMode dwShareMode,
    IntPtr lpSecurityAttributes,
    FileCreationDisposition dwCreationDisposition,
    uint dwFlagsAndAttributes,
    IntPtr hTemplateFile);

  [DllImport(Kernel32LibraryName, CharSet = Properties.BuildCharSet)]
  public static extern IntPtr CreateFile(string lpFileName,
    uint dwDesiredAccess,
    FileShareMode dwShareMode,
    ref SecurityAttributes lpSecurityAttributes,
    FileCreationDisposition dwCreationDisposition,
    uint dwFlagsAndAttributes,
    IntPtr hTemplateFile);

  #endregion
}
