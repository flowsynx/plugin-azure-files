# FlowSynx Azure Files Plugin

The Azure Files Plugin is a pre-packaged, plug-and-play integration component for the FlowSynx engine. It enables interacting with Azure File Shares to manage directories and files, supporting a variety of operations such as uploading, downloading, listing, and purging file share data. Designed for FlowSynx’s no-code/low-code automation workflows, this plugin simplifies cloud file storage integration and file management.

This plugin is automatically installed by the FlowSynx engine when selected within the platform. It is not intended for manual installation or standalone developer use outside the FlowSynx environment.

---

## Purpose

The Azure Files Plugin allows FlowSynx users to:

- Upload and download files to and from Azure File Shares.
- Manage files and directories with create, delete, and purge operations.
- List contents of directories with filtering and metadata support.
- Perform existence checks for files or folders in workflows without writing code.

---

## Supported Operations

- **create**: Creates a new file or directory in the specified path within the file share.
- **delete**: Deletes a file or directory at the specified path in the share.
- **exist**: Checks if a file or directory exists at the specified path.
- **list**: Lists files and directories under a specified path, with filtering and optional metadata.
- **purge**: Deletes all files and directories under the specified path, optionally forcing deletion.
- **read**: Reads and returns the contents of a file at the specified path.
- **write**: Writes data to a specified path in the file share, with support for overwrite.

---

## Plugin Specifications

The plugin requires the following configuration:

- `ShareName` (string): **Required.** The Azure File Share name.
- `AccountName` (string): **Optional.** The Azure Storage account name.
- `AccountKey` (string): **Optional.** The access key for the Azure Storage account.
- `ConnectionString` (string): **Optional.** The full connection string for the Azure Storage account.

### Example Configuration

```json
{
  "ShareName": "myfileshare",
  "AccountName": "myazureaccount",
  "AccountKey": "abc123xyz456==",
  "ConnectionString": ""
}
```

---

## Input Parameters

Each operation accepts specific parameters:

### Create
| Parameter     | Type    | Required | Description                                  |
|---------------|---------|----------|----------------------------------------------|
| `Path`        | string  | Yes      | The path where the new file or directory is created.|

### Delete
| Parameter     | Type    | Required | Description                                  |
|---------------|---------|----------|----------------------------------------------|
| `Path`        | string  | Yes      | The path of the file or directory to delete.|

### Exist
| Parameter     | Type    | Required | Description                                  |
|---------------|---------|----------|----------------------------------------------|
| `Path`        | string  | Yes      | The path of the file or directory to check. |

### List
| Parameter         | Type    | Required | Description                                         |
|--------------------|---------|----------|-----------------------------------------------------|
| `Path`             | string  | Yes      | The prefix path to list files and directories from. |
| `Filter`           | string  | No       | A filter pattern for file names.                    |
| `Recurse`          | bool    | No       | Whether to list recursively. Default: `false`.      |
| `CaseSensitive`    | bool    | No       | Whether the filter is case-sensitive. Default: `false`. |
| `IncludeMetadata`  | bool    | No       | Whether to include file metadata. Default: `false`. |
| `MaxResults`       | int     | No       | Maximum number of results to list. Default: `2147483647`. |

### Purge
| Parameter     | Type    | Required | Description                                           |
|---------------|---------|----------|-------------------------------------------------------|
| `Path`        | string  | Yes      | The path prefix to purge.                             |
| `Force`       | bool    | No       | Whether to force deletion without confirmation.       |

### Read
| Parameter     | Type    | Required | Description                                  |
|---------------|---------|----------|----------------------------------------------|
| `Path`        | string  | Yes      | The path of the file to read.               |

### Write
| Parameter     | Type    | Required | Description                                                        |
|---------------|---------|----------|--------------------------------------------------------------------|
| `Path`        | string  | Yes      | The path where data should be written.                            |
| `Data`        | object  | Yes      | The data to write to the Azure File Share.                        |
| `Overwrite`   | bool    | No       | Whether to overwrite if the file already exists. Default: `false`. |

### Example input (Write)

```json
{
  "Operation": "write",
  "Path": "documents/report.txt",
  "Data": "This is the report content.",
  "Overwrite": true
}
```

---

## Debugging Tips

- Verify the `ShareName`, `AccountName`, `AccountKey`, or `ConnectionString` values are correct and have sufficient permissions.
- Ensure the `Path` is valid and does not conflict with existing files or directories (especially for create/write).
- For write operations, confirm that `Data` is properly encoded or formatted for upload.
- When using list, adjust filters carefully to match file names (wildcards like `*.txt` are supported).
- Purge will fail on non-empty folders unless `Force` is set to `true`.

---

## Azure File Storage Considerations

- Case Sensitivity: File paths are case-sensitive; use the `CaseSensitive` flag in list if needed.
- Hierarchy Simulation: Azure File Shares support directories and files natively.
- Large File Uploads: For large files, uploads are automatically chunked by the plugin.
- Metadata: When using `IncludeMetadata` in list, additional API calls may increase latency.

---

## Security Notes

- The plugin uses Azure Storage SDK authentication with the provided credentials.
- No credentials or file share data are persisted outside the execution scope unless explicitly configured.
- Only authorized FlowSynx platform users can view or modify plugin configurations.

---

## License

© FlowSynx. All rights reserved.