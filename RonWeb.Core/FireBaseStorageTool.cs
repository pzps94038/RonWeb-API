using System;
using Firebase.Storage;

namespace RonWeb.Core
{
	public class FireBaseStorageTool
	{
		private FirebaseStorage _storage { get; set; }
		public FireBaseStorageTool(string storageBucket)
		{
			this._storage = new FirebaseStorage(storageBucket);
        }

		
		public async Task<string?> Upload(Stream stream, List<string> childs)
		{
			FirebaseStorageReference? root = null;
            foreach (var child in childs)
			{
				if (root == null)
				{
					root = this._storage.Child(child);
				}
				else
				{
                    root = root.Child(child);
                }
            }
			if (root != null)
			{
				var url = await root.PutAsync(stream);
				return url;
			}
			else
			{
				return null;
			}
		}
	}
}

