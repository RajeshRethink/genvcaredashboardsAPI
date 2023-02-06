using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace genvcaredashboardsAPI.Helpers
{
    public class CloudHelper
    {
        private readonly IConfiguration _configuration;

        public CloudHelper(IConfiguration configuration)
        {
            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public static async Task<string> UploadDocumentToS3(IFormFile file, string awsAccessKeyId, string awsSecretAccessKey
            , string region, string bucketName, string bucketFolderName, string awsCloudUrl, string newfilename)
        {
            string key = newfilename;
            var _awsS3Client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey, RegionEndpoint.GetBySystemName(region));

            using (var newMemoryStream = new MemoryStream())
            {
                file.CopyTo(newMemoryStream);

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = newMemoryStream,
                    Key = key,
                    BucketName = bucketName + "/" + bucketFolderName,
                    ContentType = file.ContentType
                };

                var fileTransferUtility = new TransferUtility(_awsS3Client);

                await fileTransferUtility.UploadAsync(uploadRequest);


            }
            awsCloudUrl = string.Format(awsCloudUrl, key);
            return awsCloudUrl;

        }

        public static async Task DeleteDocumentFromS3( string awsAccessKeyId, string awsSecretAccessKey
            , string region, string bucketName, string bucketFolderName, string filename)
        {
            try
            {
                var _awsS3Client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey, RegionEndpoint.GetBySystemName(region));

                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = bucketName + "/" + bucketFolderName,
                    Key =  filename
                };

                Console.WriteLine("Deleting an object");
                await _awsS3Client.DeleteObjectAsync(deleteObjectRequest);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when deleting an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when deleting an object", e.Message);
            }
        }
    }
}