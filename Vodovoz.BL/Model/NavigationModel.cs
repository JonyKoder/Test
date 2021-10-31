using System;

namespace Vodovoz.DAL.Model {
    public class NavigationModel {
        public NavigationModel(string title) => Title = title;

        public string Title { get; set; }

        public override bool Equals(object? obj) => obj is NavigationModel model && Title == model.Title;

        public override int GetHashCode() => HashCode.Combine(Title);
    }
}
