using System;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using WinApi.Core;
using WinApi.DxUtils.Core;
using Device = SharpDX.Direct3D11.Device;
using NetCoreEx.Geometry;

namespace WinApi.DxUtils.D3D11
{
  public abstract class D3D11ContainerCore : D3D11Dxgi1_2ContainerCore, IDxgi1ContainerWithSwapChain
  {
    public virtual DeviceContext Context { get; protected set; }
    public virtual RenderTargetView RenderTargetView { get; protected set; }
    public virtual DepthStencilView DepthStencilView { get; protected set; }
    public virtual Texture2D DepthStencilTexture { get; protected set; }
    public virtual SwapChain SwapChain { get; protected set; }

    public static Size GetValidatedSize(ref Size size) {
      var h = size.Height >= 0 ? size.Height : 0;
      var w = size.Width >= 0 ? size.Width : 0;
      return new Size(w, h);
    }

    protected void EnsureSwapChain() {
      if (this.SwapChain == null) this.CreateSwapChain();
    }

    protected abstract void CreateSwapChain();

    protected void EnsureContext() {
      if (this.Context == null) this.CreateContext();
    }

    protected abstract void CreateContext();

    protected void EnsureRenderTargetView() {
      if (this.RenderTargetView == null) this.CreateRenderTargetView();
    }

    protected void EnsureDepthStencilView() {
      if (this.DepthStencilView == null) this.CreateDepthStencilView();
    }

    protected abstract void CreateRenderTargetView();

    protected abstract void CreateDepthStencilView();
  }

  // ReSharper disable once InconsistentNaming
  public abstract class D3D11_1ContainerCore : D3D11ContainerCore, IDxgi1_2ContainerWithSwapChain
  {
    public override DeviceContext Context => this.Context1;

    public virtual DeviceContext1 Context1 { get; protected set; }
    public override Factory DxgiFactory => this.DxgiFactory2;
    public override SharpDX.DXGI.Device DxgiDevice => this.DxgiDevice2;
    public override SwapChain SwapChain => this.SwapChain1;
    public virtual SwapChain1 SwapChain1 { get; protected set; }
  }

  // ReSharper disable once InconsistentNaming
  public abstract class D3D11_1Container : D3D11_1ContainerCore
  {
    protected int BackBufferWidth { get; set; }
    protected int BackBufferHeight { get; set; }
    
    protected override void CreateDxgiDevice() {
      this.EnsureDevice();
      this.DxgiDevice2 = this.Device1.QueryInterface<SharpDX.DXGI.Device2>();
    }

    protected override void CreateAdapter() {
      this.EnsureDxgiDevice();
      this.Adapter = this.DxgiDevice2.GetParent<Adapter>();
    }

    protected override void CreateDxgiFactory() {
      this.EnsureAdapter();
      this.DxgiFactory2 = this.Adapter.GetParent<Factory2>();
    }

    protected override void CreateContext() {
      this.EnsureDevice();
      this.Context1 = this.Device1.ImmediateContext1;
    }

    protected override void CreateRenderTargetView() {
      this.EnsureDevice();
      this.EnsureSwapChain();
      
      var swapDesc = this.SwapChain1.Description;
      
      using (var backBuffer = this.SwapChain1.GetBackBuffer<Texture2D>(0)) {
        var renderTargetDesc = new RenderTargetViewDescription() {
          Format = Format.B8G8R8A8_UNorm,
          Dimension = swapDesc.SampleDescription.Count > 1 ? RenderTargetViewDimension.Texture2DMultisampled : RenderTargetViewDimension.Texture2D,
          Texture2D = new RenderTargetViewDescription.Texture2DResource() {
            MipSlice = 0
          }
        };

        BackBufferWidth = backBuffer.Description.Width;
        BackBufferHeight = backBuffer.Description.Height;
        
        this.RenderTargetView = new RenderTargetView(this.Device1, backBuffer, renderTargetDesc);
      }
    }

    protected override void CreateDepthStencilView() {
      this.EnsureDevice();
      this.EnsureSwapChain();
      this.EnsureRenderTargetView();

      var swapDesc = this.SwapChain1.Description;

      // Create a depth stencil texture description
      var depthStencilTexDesc = new Texture2DDescription() {
        ArraySize = 1, // Single texture
        BindFlags = BindFlags.DepthStencil, // Bind as depth stencil
        CpuAccessFlags = CpuAccessFlags.None, // No CPU access required
        Format = Format.D24_UNorm_S8_UInt, // 24-bit depth, 8-bit stencil
        Width = BackBufferWidth, // Width of the depth buffer
        Height = BackBufferHeight, // Height of the depth buffer
        MipLevels = 1, // Single mipmap level
        SampleDescription = new SampleDescription(swapDesc.SampleDescription.Count, swapDesc.SampleDescription.Quality),
        Usage = ResourceUsage.Default, // Default usage
        OptionFlags = ResourceOptionFlags.None // No additional options
      };

      // Create the depth stencil texture
      DepthStencilTexture?.Dispose();
      DepthStencilTexture = new Texture2D(this.Device1, depthStencilTexDesc);

      // Create a depth stencil view description
      var depthStencilViewDesc = new DepthStencilViewDescription() {
        Format = depthStencilTexDesc.Format,
        Dimension = DepthStencilViewDimension.Texture2D,
        Flags = DepthStencilViewFlags.None,
      };

      // Create the depth stencil view
      this.DepthStencilView = new DepthStencilView(this.Device1, DepthStencilTexture, depthStencilViewDesc);
    }

    protected void ConnectRenderTargetView() {
      this.EnsureContext();
      this.EnsureRenderTargetView();
      this.EnsureDepthStencilView();
      this.Context1.OutputMerger.SetRenderTargets(this.DepthStencilView, this.RenderTargetView);
    }

    protected void DisconnectRenderTargetView() {
      if (this.Context == null) return;
      this.Context1.OutputMerger.SetRenderTargets((DepthStencilView) null, (RenderTargetView) null);
    }
  }
}