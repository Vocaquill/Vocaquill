using Microsoft.AspNetCore.Mvc;
using NAudio.Wave;
using Vosk;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AudioToTextServer.Controllers
{
    [Route("api/audio")]
    [ApiController]
    public class AudioController : ControllerBase
    {
        private static readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        private static readonly Model _model;
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        static AudioController()
        {
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }

            Vosk.Vosk.SetLogLevel(-1);
            var modelPath = Path.Combine(Directory.GetCurrentDirectory(), "VoskModel");
            _model = new Model(modelPath);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadAudio(IFormFile file)
        {
            await _semaphore.WaitAsync();
            string filePath = null;

            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("File error");
                }

                filePath = Path.Combine(_uploadPath, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                string text;
                using (var waveStream = new WaveFileReader(filePath))
                using (var recognizer = new VoskRecognizer(_model, 16000.0f))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;

                    while ((bytesRead = waveStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        recognizer.AcceptWaveform(buffer, bytesRead);
                    }

                    text = recognizer.FinalResult();
                }

                return Ok(new { message = text });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
            finally
            {
                _semaphore.Release();

                if (filePath != null && System.IO.File.Exists(filePath))
                {
                    try
                    {
                        System.IO.File.Delete(filePath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Не вдалося видалити файл {filePath}: {ex.Message}");
                    }
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}
