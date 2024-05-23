/**
 * Response File Uploaded
 */
export interface ResponseFileUpload {
  // Id of file
  id: string;

  // Name of file
  name: string;

  // Name of origin file Name
  originalFileName: string;

  // Url
  url: string;

  // File size in bytes
  size: number;
}
