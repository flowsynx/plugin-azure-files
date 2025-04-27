﻿namespace FlowSynx.Plugins.Azure.Files.Extensions;

public static class ByteExtensions
{
    public static string ToHexString(this byte[]? bytes)
    {
        return bytes == null ? string.Empty : Convert.ToHexString(bytes);
    }
}