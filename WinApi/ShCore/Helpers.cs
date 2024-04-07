
using System;
using WinApi.Core;

namespace WinApi.ShCore;

public static class ShCoreHelpers
{
  public static HResult GetDpiForMonitor(IntPtr hmonitor, MonitorDpiType dpiType, out uint dpiX, out uint dpiY) {
    return ShCoreMethods.GetDpiForMonitor(hmonitor, dpiType, out dpiX, out dpiY);
  }

  public static HResult SetProcessDpiAwareness(ProcessDpiAwareness awareness) {
    return ShCoreMethods.SetProcessDpiAwareness(awareness);
  }

  public static void GetProcessDpiAwareness(IntPtr hprocess, out ProcessDpiAwareness awareness) {
    ShCoreMethods.GetProcessDpiAwareness(hprocess, out awareness);
  }
}
