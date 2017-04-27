namespace Notify.Code.Net
{
    /// <summary>
    /// 数据回调委托
    /// </summary>
    /// <param name="data">数据</param>
    /// <param name="state">参数</param>
    public delegate void ReceiveDataCallback(byte[] data, object state);
}