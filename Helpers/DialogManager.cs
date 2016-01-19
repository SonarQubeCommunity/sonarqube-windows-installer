//----------------------------------------------------------------------------------------------
// <copyright file="DialogManager.cs" company="SonarSource SA and Microsoft Corporation">
// Copyright (c) SonarSource SA and Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
//----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SonarQubeBootstrapper.Helpers
{
    public class DialogManager
    {
        private List<Form> dialogs = new List<Form>();
        private int currentDialogIndex;
        private static DialogManager instance;
        private Stack<Tuple<int, Form>> dialogSequenceStack = new Stack<Tuple<int, Form>>();

        private DialogManager()
        {
            currentDialogIndex = -1;
            DialogLocation = Point.Empty;
        }

        public Point DialogLocation { get; set; }

        public static DialogManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DialogManager();
                }

                return instance;
            }
        }

        public DialogManager Add<T>() where T : Form, new()
        {
            dialogs.Add(new T());
            return this;
        }

        public void First()
        {
            if (dialogs.Count != 0)
            {
                HideCurrentDialog();
                currentDialogIndex = 0;

                dialogSequenceStack.Push(new Tuple<int, Form>(currentDialogIndex, dialogs[0]));

                this.ShowDialog(dialogs[0]);
            }
        }

        public void Next()
        {
            if (currentDialogIndex + 1 < dialogs.Count)
            {
                HideCurrentDialog();
                currentDialogIndex++;

                dialogSequenceStack.Push(new Tuple<int, Form>(currentDialogIndex, dialogs.ElementAt(currentDialogIndex)));

                this.ShowDialog(dialogs.ElementAt(currentDialogIndex));
            }
        }

        public void Previous()
        {
            if (dialogSequenceStack.Count >= 2)
            {
                HideCurrentDialog();

                // Pop the current dialog.
                dialogSequenceStack.Pop();

                // Peek the previous dialog and show it.
                var indexDialogTuple = dialogSequenceStack.Peek();
                currentDialogIndex = indexDialogTuple.Item1;
                this.Show(indexDialogTuple.Item2);
            }
        }

        public void Go<T>()
        {
            for (int index = 0; index < dialogs.Count; index++)
            {
                if (dialogs[index] is T)
                {
                    HideCurrentDialog();
                    currentDialogIndex = index;

                    dialogSequenceStack.Push(new Tuple<int, Form>(currentDialogIndex, dialogs.ElementAt(currentDialogIndex)));

                    this.ShowDialog(dialogs[index]);
                }
            }
        }

        private void HideCurrentDialog()
        {
            if (currentDialogIndex >= 0 &&
                currentDialogIndex < dialogs.Count)
            {
                dialogs.ElementAt(currentDialogIndex).Hide();
            }
        }

        private void UpdateDialogLocation(Form dialog)
        {
            // If this is the very first dialog we are showing, place the first dialog
            // in the center of the screen.
            if (this.DialogLocation == Point.Empty)
            {
                dialog.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                dialog.StartPosition = FormStartPosition.Manual;
                dialog.Location = DialogLocation;
            }
        }

        private void ShowDialog(Form dialog)
        {
            UpdateDialogLocation(dialog);

            dialog.ShowDialog();
        }

        private void Show(Form dialog)
        {
            UpdateDialogLocation(dialog);

            dialog.Show();
        }
    }
}
