using System.Windows.Controls;
using System.Windows.Input;
using CryptLogic;

namespace Controllers
{
    public class TextBoxController
    {
        private ComboBox _comboBox1;
        private TextBox _inputTextBoxLab1;
        private TextBox _encryptedString;
        private TextBox _decryptedString;
        private CeaserCipher _cc;

        public TextBoxController(ComboBox comboBox1, TextBox inputTextBoxLab1, TextBox encryptedString, TextBox decryptedString)
        {
            this._comboBox1 = comboBox1;
            this._inputTextBoxLab1 = inputTextBoxLab1;
            this._encryptedString = encryptedString;
            this._decryptedString = decryptedString;
        }

        public void InputTextBoxLab1KeyDown(object sender, KeyEventArgs key)
        {
            if (key.Key == Key.Enter)
            {
                var keyComboBox = (int)_comboBox1.SelectedItem;
                var stringTextBox = _inputTextBoxLab1.Text;
                _encryptedString.Text = _cc.Encode(stringTextBox, keyComboBox);
                _decryptedString.Text = _cc.Encode(_encryptedString.Text, -keyComboBox);
            }
        }
    }
}
