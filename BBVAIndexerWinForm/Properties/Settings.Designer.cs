﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BBVAIndexerWinForm.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("62690d1a-2574-4739-8d66-e2fdfc4adde1")]
        public string LUISAppID {
            get {
                return ((string)(this["LUISAppID"]));
            }
            set {
                this["LUISAppID"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0fc0fa6f97604d12ab9f5fa276f01fe4")]
        public string LUISKey {
            get {
                return ((string)(this["LUISKey"]));
            }
            set {
                this["LUISKey"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("jmmmediaservicesstorage")]
        public string IndexedFilesStorageAccountName {
            get {
                return ((string)(this["IndexedFilesStorageAccountName"]));
            }
            set {
                this["IndexedFilesStorageAccountName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("VpmaM9avmqz7zYr/CCtMUndgZserSIE26sYRS/IqInzlPC03KW9BR+DHIOGV/AOPYxZMnTMSOtf1jOojs" +
            "8OS/Q==")]
        public string IndexedFilesStorageAccountKey {
            get {
                return ((string)(this["IndexedFilesStorageAccountKey"]));
            }
            set {
                this["IndexedFilesStorageAccountKey"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("json")]
        public string IndexedFilesExtension {
            get {
                return ((string)(this["IndexedFilesExtension"]));
            }
            set {
                this["IndexedFilesExtension"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool VerboseLogging {
            get {
                return ((bool)(this["VerboseLogging"]));
            }
            set {
                this["VerboseLogging"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("400")]
        public int LUISMaxCharacters {
            get {
                return ((int)(this["LUISMaxCharacters"]));
            }
            set {
                this["LUISMaxCharacters"] = value;
            }
        }
    }
}
