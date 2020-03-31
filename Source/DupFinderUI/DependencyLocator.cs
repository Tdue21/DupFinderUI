// ****************************************************************************
// * MIT License
// *
// * Copyright (c) 2020 Thomas Due
// *
// * Permission is hereby granted, free of charge, to any person obtaining a copy
// * of this software and associated documentation files (the "Software"), to deal
// * in the Software without restriction, including without limitation the rights
// * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// * copies of the Software, and to permit persons to whom the Software is
// * furnished to do so, subject to the following conditions:
// *
// * The above copyright notice and this permission notice shall be included in all
// * copies or substantial portions of the Software.
// *
// * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// * SOFTWARE.
// ****************************************************************************

using DupFinderUI.Interfaces;
using DupFinderUI.Services;
using DupFinderUI.ViewModels;
using Unity;

namespace DupFinderUI
{
    /// <summary>
    /// </summary>
    public class DependencyLocator
    {
        private readonly IUnityContainer _container;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DependencyLocator" /> class.
        /// </summary>
        public DependencyLocator() =>
            _container = new UnityContainer()
                         .RegisterType<IFileSystemService, FileSystemService>()
                         .RegisterType<IProcessService, ProcessService>()
                         .RegisterType<ISettingsService, SettingsService>()
                         .RegisterType<IDupFinderService, DupFinderService>()
                         .RegisterType<MainViewModel>();

        /// <summary>
        ///     Gets the main view model.
        /// </summary>
        /// <value>
        ///     The main view model.
        /// </value>
        public MainViewModel MainViewModel => _container.Resolve<MainViewModel>();
    }
}
