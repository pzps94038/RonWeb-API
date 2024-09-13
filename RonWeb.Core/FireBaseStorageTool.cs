using System;
using Firebase.Storage;

namespace RonWeb.Core
{
	public class FireBaseStorageUrl
	{
		public string Path { get; set; } = string.Empty;
		public string Url { get; set; } = string.Empty;
    }

    public class FireBaseStorageTool
	{
		private FirebaseStorage _storage { get; set; }
		public FireBaseStorageTool(string storageBucket)
		{
			_storage = new FirebaseStorage(storageBucket);
        }

		
		public async Task<FireBaseStorageUrl?> Upload(Stream stream, string path)
		{
			if (path != null)
			{
				var url = await _storage.Child(path).PutAsync(stream);
				return new FireBaseStorageUrl
				{
					Path = path,
					Url = url
				};
			}
			else
			{
				return null;
			}
		}


        public async Task Delete(string path)
        {
            if (path != null)
            {
				await _storage.Child(path).DeleteAsync();
            }
        }
    }
}

