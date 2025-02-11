using NAudio.Wave;
using NAudio.CoreAudioApi;
using System.IO;

namespace Vocaquill.AllWindow.Additionals
{
    public class AudioRecorder
    {
        public AudioRecorder()
        {
            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Records"));

            _convertedFilePath = Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("Records", "recorded_audio_16khz.wav")); ;
            _outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("Records", "recorded_audio.wav"));
        }
        public Task StartRecording()
        {
            var device = new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            _capture = new WasapiLoopbackCapture(device);

            _writer = new WaveFileWriter(_outputFilePath, _capture.WaveFormat);

            _recordingStoppedTcs = new TaskCompletionSource<bool>();

            _capture.DataAvailable += (s, a) => _writer.Write(a.Buffer, 0, a.BytesRecorded);

            _capture.RecordingStopped += (s, a) =>
            {
                _writer?.Dispose();
                _capture?.Dispose();

                ConvertWavTo16KHzMono(_outputFilePath, _convertedFilePath);
                DeleteOriginalFile(_outputFilePath);
                _recordingStoppedTcs.SetResult(true);
            };

            _capture.StartRecording();
            return Task.CompletedTask;
        }

        public async Task StopRecording()
        {
            if (_capture != null)
            {
                _capture.StopRecording();
                Thread.Sleep(500);
                await _recordingStoppedTcs.Task;
            }
        }
        private void ConvertWavTo16KHzMono(string inputFile, string outputFile)
        {
            using (var reader = new AudioFileReader(inputFile))
            {
                var newFormat = new WaveFormat(16000, 16, 1);
                using (var resampler = new MediaFoundationResampler(reader, newFormat))
                {
                    resampler.ResamplerQuality = 60;
                    WaveFileWriter.CreateWaveFile(outputFile, resampler);
                }
            }
        }

        private void DeleteOriginalFile(string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
            else
                throw new Exception();
        }

        private WasapiLoopbackCapture _capture;
        private WaveFileWriter _writer;
        private string _outputFilePath;
        private string _convertedFilePath;
        private TaskCompletionSource<bool> _recordingStoppedTcs;
    }
}
