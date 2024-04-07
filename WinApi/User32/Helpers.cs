using System;
using System.Runtime.InteropServices;
using NetCoreEx.Geometry;
using WinApi.Core;
using WinApi.Kernel32;
using WinApi.ShCore;

namespace WinApi.User32;

public static class User32Helpers
{
  public static IntPtr GetWindowLongPtr(IntPtr hwnd, WindowLongFlags nIndex) {
    return User32Methods.GetWindowLongPtr(hwnd, (int) nIndex);
  }

  public static IntPtr SetWindowLongPtr(IntPtr hwnd, WindowLongFlags nIndex, IntPtr dwNewLong) {
    return User32Methods.SetWindowLongPtr(hwnd, (int) nIndex, dwNewLong);
  }

  public static IntPtr GetClassLongPtr(IntPtr hwnd, ClassLongFlags nIndex) {
    return User32Methods.GetClassLongPtr(hwnd, (int) nIndex);
  }

  public static IntPtr SetClassLongPtr(IntPtr hwnd, ClassLongFlags nIndex, IntPtr dwNewLong) {
    return User32Methods.SetClassLongPtr(hwnd, (int) nIndex, dwNewLong);
  }

  public static bool GetClassInfoEx(IntPtr hInstance, string lpClassName,
    out WindowClassEx classInfo) {
    WindowClassExBlittable classExBlittable;
    if (User32Methods.GetClassInfoEx(hInstance, lpClassName, out classExBlittable)) {
      classInfo = new WindowClassEx {
        Size = classExBlittable.Size,
        BackgroundBrushHandle = classExBlittable.BackgroundBrushHandle,
        ClassExtraBytes = classExBlittable.ClassExtraBytes,
        CursorHandle = classExBlittable.CursorHandle,
        InstanceHandle = classExBlittable.InstanceHandle,
        Styles = classExBlittable.Styles,
        IconHandle = classExBlittable.IconHandle,
        SmallIconHandle = classExBlittable.SmallIconHandle,
        WindowExtraBytes = classExBlittable.WindowExtraBytes,
        WindowProc = Marshal.GetDelegateForFunctionPointer<WindowProc>(classExBlittable.WindowProc),
        ClassName = Marshal.SystemDefaultCharSize == 1
          ? Marshal.PtrToStringAnsi(classExBlittable.ClassName)
          : Marshal.PtrToStringUni(classExBlittable.ClassName)
        // Menu name left out, since GetClassInfo doesn't return it
      };
      return true;
    }

    classInfo = new WindowClassEx();
    return false;
  }

  public static IntPtr LoadIcon(IntPtr hInstance, SystemIcon icon) {
    return User32Methods.LoadIcon(hInstance, new IntPtr((int) icon));
  }

  public static IntPtr LoadCursor(IntPtr hInstance, SystemCursor cursor) {
    return User32Methods.LoadCursor(hInstance, new IntPtr((int) cursor));
  }

  public static int DrawText(IntPtr hdc, string lpString, int nCount, ref Rectangle lpRect,
    DrawTextFormatFlags uFormat) {
    return User32Methods.DrawText(hdc, lpString, nCount, ref lpRect, (uint) uFormat);
  }

  public static bool SetWindowPos(IntPtr hwnd, HwndZOrder order, int x, int y, int cx, int cy,
    WindowPositionFlags flags) {
    return User32Methods.SetWindowPos(hwnd, new IntPtr((int) order), x, y, cx, cy, flags);
  }

  public static bool PeekMessage(out Message lpMsg, IntPtr hWnd, uint wMsgFilterMin,
    uint wMsgFilterMax, PeekMessageFlags wRemoveMsg) {
    return User32Methods.PeekMessage(out lpMsg, hWnd, wMsgFilterMin, wMsgFilterMax, (uint) wRemoveMsg);
  }

  public static IntPtr GetNextWindow(IntPtr hwnd, GetWindowFlag cmd) {
    return User32Methods.GetNextWindow(hwnd, (uint) cmd);
  }

  public static IntPtr GetWindow(IntPtr hwnd, GetWindowFlag cmd) {
    return User32Methods.GetWindow(hwnd, (uint) cmd);
  }

  public static bool SystemParametersInfo(SystemParametersAccessibilityInfo uiAction, uint uiParam, IntPtr pvParam,
    SystemParamtersInfoFlags fWinIni) {
    return User32Methods.SystemParametersInfo((uint) uiAction, uiParam, pvParam, fWinIni);
  }

  public static bool SystemParametersInfo(SystemParametersDesktopInfo uiAction, uint uiParam, IntPtr pvParam,
    SystemParamtersInfoFlags fWinIni) {
    return User32Methods.SystemParametersInfo((uint) uiAction, uiParam, pvParam, fWinIni);
  }

  public static bool SystemParametersInfo(SystemParametersIconInfo uiAction, uint uiParam, IntPtr pvParam,
    SystemParamtersInfoFlags fWinIni) {
    return User32Methods.SystemParametersInfo((uint) uiAction, uiParam, pvParam, fWinIni);
  }

  public static bool SystemParametersInfo(SystemParametersInputInfo uiAction, uint uiParam, IntPtr pvParam,
    SystemParamtersInfoFlags fWinIni) {
    return User32Methods.SystemParametersInfo((uint) uiAction, uiParam, pvParam, fWinIni);
  }

  public static bool SystemParametersInfo(SystemParametersMenuInfo uiAction, uint uiParam, IntPtr pvParam,
    SystemParamtersInfoFlags fWinIni) {
    return User32Methods.SystemParametersInfo((uint) uiAction, uiParam, pvParam, fWinIni);
  }

  public static bool SystemParametersInfo(SystemParametersPowerInfo uiAction, uint uiParam, IntPtr pvParam,
    SystemParamtersInfoFlags fWinIni) {
    return User32Methods.SystemParametersInfo((uint) uiAction, uiParam, pvParam, fWinIni);
  }

  public static bool SystemParametersInfo(SystemParametersScreenSaverInfo uiAction, uint uiParam, IntPtr pvParam,
    SystemParamtersInfoFlags fWinIni) {
    return User32Methods.SystemParametersInfo((uint) uiAction, uiParam, pvParam, fWinIni);
  }

  public static bool SystemParametersInfo(SystemParametersTimeoutInfo uiAction, uint uiParam, IntPtr pvParam,
    SystemParamtersInfoFlags fWinIni) {
    return User32Methods.SystemParametersInfo((uint) uiAction, uiParam, pvParam, fWinIni);
  }

  public static bool SystemParametersInfo(SystemParametersUiEffectsInfo uiAction, uint uiParam, IntPtr pvParam,
    SystemParamtersInfoFlags fWinIni) {
    return User32Methods.SystemParametersInfo((uint) uiAction, uiParam, pvParam, fWinIni);
  }

  public static bool SystemParametersInfo(SystemParametersWindowInfo uiAction, uint uiParam, IntPtr pvParam,
    SystemParamtersInfoFlags fWinIni) {
    return User32Methods.SystemParametersInfo((uint) uiAction, uiParam, pvParam, fWinIni);
  }

  public static MessageBoxResult MessageBox(IntPtr hWnd, string lpText, string lpCaption, MessageBoxFlags type) {
    return User32Methods.MessageBox(hWnd, lpText, lpCaption, (uint) type);
  }

  public static MessageBoxResult MessageBox(string message, string title = "Info",
    MessageBoxFlags flags = MessageBoxFlags.MB_OK, IntPtr parent = default(IntPtr)) {
    return MessageBox(parent, message, title, flags);
  }

  public static uint SendInput(Input[] inputs) {
    var len = (uint) inputs.Length;
    var size = Marshal.SizeOf<Input>();
    var gcHandle = GCHandle.Alloc(inputs, GCHandleType.Pinned);
    try { return User32Methods.SendInput(len, gcHandle.AddrOfPinnedObject(), size); } finally { gcHandle.Free(); }
  }

  public static unsafe uint SendInput(ref Input input) {
    fixed (Input* ptr = &input) return User32Methods.SendInput(1, new IntPtr(ptr), Marshal.SizeOf<Input>());
  }

  public static unsafe bool GetTitleBarInfo(IntPtr hwnd, ref TitleBarInfo pti) {
    if (pti.Size == 0) pti.Size = (uint) Marshal.SizeOf<TitleBarInfo>();
    fixed (TitleBarInfo* ptr = &pti) return User32Methods.GetTitleBarInfo(hwnd, new IntPtr(ptr));
  }

  public static unsafe int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, ref Point point) {
    fixed (Point* ptr = &point) return User32Methods.MapWindowPoints(hWndFrom, hWndTo, new IntPtr(ptr), 1);
  }

  public static unsafe int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, ref Rectangle rect) {
    fixed (Rectangle* ptr = &rect) {
      var ptPtr = (Point*) ptr;
      return User32Methods.MapWindowPoints(hWndFrom, hWndTo, new IntPtr(ptPtr), 2);
    }
  }

  public static unsafe bool GetMonitorInfo(IntPtr hMonitor, out MonitorInfo lpmi) {
    lpmi = new MonitorInfo {Size = (uint) sizeof(MonitorInfo)};
    return User32Methods.GetMonitorInfo(hMonitor, ref lpmi);
  }

  public static bool InverseAdjustWindowRectEx(
    ref Rectangle lpRect, WindowStyles dwStyle, bool hasMenu,
    WindowExStyles dwExStyle) {
    var rc = new Rectangle();
    var res = User32Methods.AdjustWindowRectEx(ref rc, dwStyle, hasMenu, dwExStyle);
    if (res) Rectangle.Subtract(ref lpRect, ref rc);
    return res;
  }

  /// <summary>
  ///  Sets the DPI awareness. If not available on the current OS, it falls back to the next possible.
  /// </summary>
  /// <returns>true/false - If the process DPI awareness is successfully set, returns true. Otherwise false.</returns>
  public static bool SetProcessDpiAwarenessContext(HighDpiMode highDpiMode) {
    if (Kernel32Helpers.OsVersion.IsWindows10_1703OrGreater) {
      // SetProcessIntPtr needs Windows 10 RS2 and above
      IntPtr rs2AndAboveDpiFlag;
      switch (highDpiMode) {
        case HighDpiMode.SYSTEM_AWARE:
          rs2AndAboveDpiFlag = DPI_AWARENESS_CONTEXT.SYSTEM_AWARE;
          break;
        case HighDpiMode.PER_MONITOR_AWARE:
          rs2AndAboveDpiFlag = DPI_AWARENESS_CONTEXT.PER_MONITOR_AWARE;
          break;
        case HighDpiMode.PER_MONITOR_AWARE2:
          // Necessary for RS1, since this SetProcessIntPtr IS available here.
          rs2AndAboveDpiFlag = User32Methods.IsValidDpiAwarenessContext(DPI_AWARENESS_CONTEXT.PER_MONITOR_AWARE_V2) ? DPI_AWARENESS_CONTEXT.PER_MONITOR_AWARE_V2 : DPI_AWARENESS_CONTEXT.SYSTEM_AWARE;
          break;
        case HighDpiMode.UNAWARE_GDI_SCALED:
          // Let's make sure, we do not try to set a value which has been introduced in later Windows releases.
          rs2AndAboveDpiFlag = User32Methods.IsValidDpiAwarenessContext(DPI_AWARENESS_CONTEXT.UNAWARE_GDISCALED) ? DPI_AWARENESS_CONTEXT.UNAWARE_GDISCALED : DPI_AWARENESS_CONTEXT.UNAWARE;
          break;
        default:
          rs2AndAboveDpiFlag = DPI_AWARENESS_CONTEXT.UNAWARE;
          break;
      }

      return User32Methods.SetProcessDpiAwarenessContext(rs2AndAboveDpiFlag);
    } else if (Kernel32Helpers.OsVersion.IsWindows8_1OrGreater) {
      // 8.1 introduced SetProcessDpiAwareness
      ShCore.ProcessDpiAwareness dpiFlag;
      switch (highDpiMode) {
        case HighDpiMode.UNAWARE:
        case HighDpiMode.UNAWARE_GDI_SCALED:
          dpiFlag = ShCore.ProcessDpiAwareness.Process_DPI_Unaware;
          break;
        case HighDpiMode.SYSTEM_AWARE:
          dpiFlag = ShCore.ProcessDpiAwareness.Process_System_DPI_Aware;
          break;
        case HighDpiMode.PER_MONITOR_AWARE:
        case HighDpiMode.PER_MONITOR_AWARE2:
          dpiFlag = ShCore.ProcessDpiAwareness.Process_Per_Monitor_DPI_Aware;
          break;
        default:
          dpiFlag = ShCore.ProcessDpiAwareness.Process_System_DPI_Aware;
          break;
      }

      return ShCoreHelpers.SetProcessDpiAwareness(dpiFlag) == HResult.S_OK;
    } else {
      // Vista or higher has SetProcessDPIAware
      ShCore.ProcessDpiAwareness dpiFlag = (ShCore.ProcessDpiAwareness) (-1);
      switch (highDpiMode) {
        case HighDpiMode.UNAWARE:
        case HighDpiMode.UNAWARE_GDI_SCALED:
          // We can return, there is nothing to set if we assume we're already in DpiUnaware.
          return true;
        case HighDpiMode.SYSTEM_AWARE:
        case HighDpiMode.PER_MONITOR_AWARE:
        case HighDpiMode.PER_MONITOR_AWARE2:
          dpiFlag = ShCore.ProcessDpiAwareness.Process_System_DPI_Aware;
          break;
      }

      if (dpiFlag == ShCore.ProcessDpiAwareness.Process_System_DPI_Aware) {
        return User32Methods.SetProcessDPIAware();
      }
    }

    return false;
  }
}
