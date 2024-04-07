using System;
using System.Runtime.InteropServices;
using WinApi.Core;

namespace WinApi.ShCore;

public static class ShCoreMethods
{
  public const string LibraryName = "shcore";

  [DllImport(LibraryName, ExactSpelling = true)]
  public static extern HResult GetDpiForMonitor(IntPtr hmonitor, MonitorDpiType dpiType, out uint dpiX, out uint dpiY);

  [DllImport(LibraryName, ExactSpelling = true, SetLastError = true)]
  public static extern HResult SetProcessDpiAwareness(ProcessDpiAwareness awareness);

  [DllImport(LibraryName, ExactSpelling = true, SetLastError = true)]
  public static extern void GetProcessDpiAwareness(IntPtr hprocess, out ProcessDpiAwareness awareness);
}
