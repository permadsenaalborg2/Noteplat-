using Avalonia.Platform.Storage;
using Microsoft.VisualBasic;
using Noteplat.Models;
using Noteplat.Views;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Threading.Tasks;
namespace Noteplat.ViewModels;

public class MainViewModel : ViewModelBase
{
    public ReactiveCommand<Unit, Unit> SaveCommand { get; set; }
    public ReactiveCommand<Unit, Unit> LoadCommand { get; set; }
    public ReactiveCommand<Unit, Unit> ReverseCommand { get; set; }

    TextDocument _textDocument = new TextDocument();
    public TextDocument TextDocument
    {
        get => _textDocument;
        set => this.RaiseAndSetIfChanged(ref _textDocument, value, nameof(TextDocument));
    }
    public MainViewModel(IRepository repository) : base(repository)
    {
        SaveCommand = ReactiveCommand.CreateFromTask(saveCommand);
        LoadCommand = ReactiveCommand.CreateFromTask(loadCommand);
        ReverseCommand = ReactiveCommand.Create(reverseCommand);
    }

    async Task loadCommand()
    {
        string path = await _repository.PickFileLoad();
        if (_repository.Exists(path))
        {
            TextDocument = new TextDocument()
            {
                Filename = path,
                Text = _repository.ReadAllText(path)
            };
        }
    }
    async Task saveCommand()
    {
        if (_textDocument.Filename == String.Empty)
        {
            _textDocument.Filename = await _repository.PickFileSave();

        }
        _repository.WriteAllText(_textDocument.Filename, _textDocument.Text);
    }

    public void reverseCommand()
    {
        var list = TextDocument.Text.Split(Environment.NewLine).ToList();
        list.Reverse();
        TextDocument = new TextDocument()
        {
            Filename = TextDocument.Filename,
            Text = String.Join(Environment.NewLine, list)
        };
    }
}
