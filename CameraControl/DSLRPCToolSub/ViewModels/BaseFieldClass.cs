﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace DSLR_Tool_PC.ViewModels
{
    public class BaseFieldClass : INotifyPropertyChanged
    {
        
        #region Implementation of IEditableObject

        private Hashtable props = null;

        public BaseFieldClass()
        {
        }

        public virtual void BeginEdit()
        {
            PropertyInfo[] properties = (GetType()).GetProperties
                (BindingFlags.Public | BindingFlags.Instance);

            props = new Hashtable(properties.Length - 1);

            for (int i = 0; i < properties.Length; i++)
            {
                //check if there is set accessor

                if (null != properties[i].GetSetMethod())
                {
                    object value = properties[i].GetValue(this, null);
                    props.Add(properties[i].Name, value);
                }
            }
        }

        public virtual void EndEdit()
        {
            props = null;
        }

        public virtual void CancelEdit()
        {
            //check for inappropriate call sequence

            if (null == props) return;

            //restore old values

            PropertyInfo[] properties = (GetType()).GetProperties
                (BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < properties.Length; i++)
            {
                //check if there is set accessor

                if (null != properties[i].GetSetMethod())
                {
                    object value = props[properties[i].Name];
                    properties[i].SetValue(this, value, null);
                    NotifyPropertyChanged(properties[i].Name);
                }
            }

            //delete current values

            props = null;
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public virtual event PropertyChangedEventHandler PropertyChanged;
        public virtual void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
