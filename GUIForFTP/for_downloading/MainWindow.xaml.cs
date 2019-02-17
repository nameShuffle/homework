ion(intPort, addres.Text.ToString());
            }
        }

        /// <summary>
        /// Обработчик двойного нажатия на элемент ListBox. Если данный
        /// элемент является файлом, то ничего не проиходит. Если элемент - 
        /// директория, то обновляется информация в ListBox.
        /// </summary>
        private async void ObjectsList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = (sender as ListBox).SelectedItem;
            var objectInfo = item as ObjectInfo;

            if (objectInfo.IsDir)
            {
                await (DataContext as ClientViewModel).GoToDirectory(this.currentPort, 
                    this.currentAddres, objectInfo.FullPath);
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку скачивания. Если никакой элемент
        /// ListBox-а не выделен в настоящий момент, то ничего не проиходит.
        /// Аналогично если выделенный элемент - директория. Если это файл, то
        /// производится скачивание этого файла.
        /// </summary>
        private void ButtonDownload_Click(object sender, RoutedEventArgs e)
        {
            var item = ObjectsList.SelectedItem;
            if (item != null)
            {
                var objectInfo = item as ObjectInfo;
                if (!objectInfo.IsDir)
                {
                    (DataContext as ClientViewModel).DownloadOneFile(this.currentPort,
                        this.currentAddres, objectInfo, PathToDownload.Text.ToString(),
                        Dispatcher);
                }
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку "скачать все". Производится скачивание
        /// всех файлов, лежащих в текущей директории.
        /// </summary>
        private void ButtonDownloadAll_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as ClientViewModel).DownloadAllFiles(this.currentPort,
                this.currentAddres, PathToDownload.Text.ToString(), Dispatcher);
        }
    }
}
