using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MVVMApp.Model
{
    internal class TodoItem : ObservableObject
    {
        private string _title = "";
        private bool _isDone;
        private DateTime? _deadLine;

        [Key]
        public int Id { get; set; }

        [Required, StringLength(100, MinimumLength = 1)]
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public bool IsDone
        {
            get => _isDone;
            set => SetProperty(ref _isDone, value);
        }

        public DateTime? DeadLine
        {
            get => _deadLine;
            set => SetProperty(ref _deadLine, value);
        }
    }
}
