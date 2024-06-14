using Noteplat.Models;
using Noteplat.ViewModels;
using ReactiveUI;
using System.Collections.Generic;
using System.Text;
namespace Noteplat.Tests
{

    public class MainViewModelTests
    {

        IRepository _repository = new UnitTestRepository();

        private static string GenerateLargeText(int length)
        {
            // Define the characters to use in the text
            string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            // Create a random number generator
            Random random = new Random();

            // Build the large text string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(characters[random.Next(characters.Length)]);
            }

            return sb.ToString();
        }

        [Fact]
        public async void TestFileSystem()
        {
            var filename = Path.GetTempFileName();
            var contents = "Hello";
            _repository.WriteAllText(filename, contents);

            var pickedFile = await _repository.PickFileLoad();
        
            Assert.Equal(filename, pickedFile);
        }


        [Fact]
        public void SaveText()
        {
            // arrange
            var filename = Path.GetTempFileName();
            Assert.False(_repository.Exists(filename));

            MainViewModel mv = new(_repository);

            var contents = GenerateLargeText(10000);
            mv.TextDocument = new TextDocument(filename, contents);
            Assert.Equal(filename, mv.TextDocument.Filename);

            // act
            mv.SaveCommand.Execute().Subscribe();

            // Assert
            Assert.True(_repository.Exists(filename));

            var new_contents = _repository.ReadAllText(filename);
            Assert.Equal(new_contents, new_contents);

            _repository.Delete(filename);
            Assert.False(_repository.Exists(filename));

        }

        [Fact]
        public void LoadText()
        {
            var filename = Path.GetTempFileName();
            var contents = "Hello";
            _repository.WriteAllText(filename, contents);

            MainViewModel mv = new(_repository);

            mv.LoadCommand.Execute().Subscribe();

            Assert.Equal(contents, mv.TextDocument.Text);
        }

        [Fact]
        public void ReverseText()
        {
            var filename = Path.GetTempFileName();
            var contents = "One" + Environment.NewLine + "Two" + Environment.NewLine + "Three" + Environment.NewLine;

            MainViewModel mv = new(_repository);
            mv.TextDocument = new TextDocument()
            {
                Filename = filename,
                Text = contents
            };

            mv.ReverseCommand.Execute().Subscribe();
            Assert.NotEqual(contents, mv.TextDocument.Text);

            mv.ReverseCommand.Execute().Subscribe();
            Assert.Equal(contents, mv.TextDocument.Text);
        }
    }
}