using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Template.Domain.Dto.StreamDto
{
    public class StreamFormFile : IFormFile
    {
        private readonly Stream _stream;
        private readonly long _length;
        private readonly string _contentType;
        private readonly string _name;
        private readonly string _fileName;

        public StreamFormFile(Stream stream, long length, string contentType, string name, string fileName)
        {
            _stream = stream;
            _length = length;
            _contentType = contentType;
            _name = name;
            _fileName = fileName;
        }

        public string ContentDisposition => $"form-data; name=\"{_name}\"; filename=\"{_fileName}\"";

        public string ContentType => _contentType;

        public string FileName => _fileName;

        public IHeaderDictionary Headers => new HeaderDictionary();

        public long Length => _length;

        public string Name => _name;

        public Stream OpenReadStream() => _stream;

        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            return _stream.CopyToAsync(target, cancellationToken);
        }

        public void CopyTo(Stream target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            _stream.CopyTo(target);
        }
    }
}
