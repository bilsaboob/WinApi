using System;
using System.Runtime.InteropServices;
using NetCoreEx.BinaryExtensions;
using WinApi.Kernel32;

namespace WinApi.Kernel32;

public static class Kernel32Helpers
{
  public static Version GetVersion() {
    var dwVersion = Kernel32Methods.GetVersion();
    var build = dwVersion < 0x80000000 ? dwVersion.HighAsUInt() : 0; // (DWORD) (HIWORD(dwVersion))
    var v = new Version(
      (byte) dwVersion.Low(), // (DWORD)(LOBYTE(LOWORD(dwVersion)))
      (dwVersion.Low() >> 8) & 0xff,
      (int) build // (DWORD)(HIBYTE(LOWORD(dwVersion)))
    );
    return v;
  }

  public static bool IsWin8OrGreater(Version version = null) {
    if (version == null) version = GetVersion();
    if (version.Major > 5) {
      if ((version.Major > 6) || (version.Minor > 1)) return true;
    }

    return false;
  }

  public static bool IsWin8Point1OrGreater(Version version = null) {
    if (version == null) version = GetVersion();
    if (version.Major > 5) {
      if ((version.Major > 6) || (version.Minor > 2)) return true;
    }

    return false;
  }

  public static bool IsWin10OrGreater(Version version = null) {
    if (version == null) version = GetVersion();
    return version.Major > 6;
  }

  public static bool GetIsProcessorAMD64() {
    SystemInfo info;
    Kernel32Methods.GetNativeSystemInfo(out info);
    return info.ProcessorArchitecture == (uint) ProcessArchitecture.PROCESSOR_ARCHITECTURE_AMD64;
  }

  public static class ProcessUserModeExceptionFilter
  {
    const uint PROCESS_CALLBACK_FILTER_ENABLED = 0x1;

    [DllImport(Kernel32Methods.Kernel32LibraryName)]
    public static extern bool SetProcessUserModeExceptionPolicy(uint dwFlags);

    [DllImport(Kernel32Methods.Kernel32LibraryName)]
    public static extern bool GetProcessUserModeExceptionPolicy(out uint dwFlags);

    private static bool IsApiAvailable() {
      if (GetIsProcessorAMD64()) {
        var ver = GetVersion();
        return (ver.Major == 6) && (ver.Minor == 1) && (ver.Build >= 7601);
      }

      return false;
    }

    public static bool Disable() {
      if (IsApiAvailable()) {
        uint dwFlags;
        if (GetProcessUserModeExceptionPolicy(out dwFlags)) {
          // Turn off the bit
          dwFlags &= ~PROCESS_CALLBACK_FILTER_ENABLED;
          return SetProcessUserModeExceptionPolicy(dwFlags);
        }
      }

      return false;
    }

    public static bool Enable() {
      if (IsApiAvailable()) {
        uint dwFlags;
        if (GetProcessUserModeExceptionPolicy(out dwFlags)) {
          // Turn off the bit
          dwFlags |= PROCESS_CALLBACK_FILTER_ENABLED;
          return SetProcessUserModeExceptionPolicy(dwFlags);
        }
      }

      return false;
    }
  }

  public static class OsVersion
  {
    private static RTL_OSVERSIONINFOEX s_versionInfo = InitVersion();

    private static RTL_OSVERSIONINFOEX InitVersion() {
      // We use RtlGetVersion as it isn't subject to version lie. GetVersion
      // won't tell you the real version unless the launching exe is manifested
      // with the latest OS version.
      RTL_OSVERSIONINFOEX info = new RTL_OSVERSIONINFOEX();
      Kernel32Methods.RtlGetVersion(ref info);
      return info;
    }

    /// <summary>
    ///  Is Windows 10 Anniversary Update or later. (Redstone 1, build 14393, version 1607)
    /// </summary>
    public static bool IsWindows10_1607OrGreater
      => s_versionInfo.dwMajorVersion >= 10 && s_versionInfo.dwBuildNumber >= 14393;

    /// <summary>
    ///  Is Windows 10 Creators Update or later. (Redstone 2, build 15063, version 1703)
    /// </summary>
    public static bool IsWindows10_1703OrGreater
      => s_versionInfo.dwMajorVersion >= 10 && s_versionInfo.dwBuildNumber >= 15063;

    /// <summary>
    ///  Is Windows 8.1 or later.
    /// </summary>
    public static bool IsWindows8_1OrGreater
      => s_versionInfo.dwMajorVersion >= 10
         || (s_versionInfo.dwMajorVersion == 6 && s_versionInfo.dwMinorVersion == 3);

  }
}
