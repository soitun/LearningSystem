namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Organization 主键列：Org_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Organization : WeiSha.Data.Entity {
    		
    		protected Int32 _Org_ID;
    		
    		protected Int32 _Olv_ID;
    		
    		protected String _Olv_Name;
    		
    		protected String _Org_AbbrEnName;
    		
    		protected String _Org_AbbrName;
    		
    		protected String _Org_Address;
    		
    		protected String _Org_BankAcc;
    		
    		protected String _Org_City;
    		
    		protected String _Org_CoBank;
    		
    		protected String _Org_Config;
    		
    		protected Int32 _Org_CourseCount;
    		
    		protected String _Org_Description;
    		
    		protected String _Org_District;
    		
    		protected String _Org_Email;
    		
    		protected String _Org_EnName;
    		
    		protected String _Org_ExtraMobi;
    		
    		protected String _Org_ExtraWeb;
    		
    		protected String _Org_Fax;
    		
    		protected String _Org_GonganBeian;
    		
    		protected String _Org_ICP;
    		
    		protected String _Org_Intro;
    		
    		protected Boolean _Org_IsDefault;
    		
    		protected Boolean _Org_IsPass;
    		
    		protected Boolean _Org_IsRoot;
    		
    		protected Boolean _Org_IsShow;
    		
    		protected Boolean _Org_IsUse;
    		
    		protected String _Org_Keywords;
    		
    		protected String _Org_Lang;
    		
    		protected String _Org_Latitude;
    		
    		protected String _Org_Linkman;
    		
    		protected String _Org_LinkmanPhone;
    		
    		protected String _Org_LinkmanQQ;
    		
    		protected String _Org_Logo;
    		
    		protected String _Org_Longitude;
    		
    		protected String _Org_Name;
    		
    		protected String _Org_Owner;
    		
    		protected String _Org_Phone;
    		
    		protected String _Org_PlatformName;
    		
    		protected String _Org_Province;
    		
    		protected Int32 _Org_QuesCount;
    		
    		protected DateTime _Org_RegTime;
    		
    		protected String _Org_Street;
    		
    		protected String _Org_Template;
    		
    		protected String _Org_TemplateMobi;
    		
    		protected String _Org_TwoDomain;
    		
    		protected String _Org_USCI;
    		
    		protected String _Org_WebSite;
    		
    		protected String _Org_Weixin;
    		
    		protected String _Org_Zip;
    		
    		public Int32 Org_ID {
    			get {
    				return this._Org_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_ID, _Org_ID, value);
    				this._Org_ID = value;
    			}
    		}
    		
    		public Int32 Olv_ID {
    			get {
    				return this._Olv_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Olv_ID, _Olv_ID, value);
    				this._Olv_ID = value;
    			}
    		}
    		
    		public String Olv_Name {
    			get {
    				return this._Olv_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Olv_Name, _Olv_Name, value);
    				this._Olv_Name = value;
    			}
    		}
    		
    		public String Org_AbbrEnName {
    			get {
    				return this._Org_AbbrEnName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_AbbrEnName, _Org_AbbrEnName, value);
    				this._Org_AbbrEnName = value;
    			}
    		}
    		
    		public String Org_AbbrName {
    			get {
    				return this._Org_AbbrName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_AbbrName, _Org_AbbrName, value);
    				this._Org_AbbrName = value;
    			}
    		}
    		
    		public String Org_Address {
    			get {
    				return this._Org_Address;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_Address, _Org_Address, value);
    				this._Org_Address = value;
    			}
    		}
    		
    		public String Org_BankAcc {
    			get {
    				return this._Org_BankAcc;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_BankAcc, _Org_BankAcc, value);
    				this._Org_BankAcc = value;
    			}
    		}
    		
    		public String Org_City {
    			get {
    				return this._Org_City;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_City, _Org_City, value);
    				this._Org_City = value;
    			}
    		}
    		
    		public String Org_CoBank {
    			get {
    				return this._Org_CoBank;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_CoBank, _Org_CoBank, value);
    				this._Org_CoBank = value;
    			}
    		}
    		
    		public String Org_Config {
    			get {
    				return this._Org_Config;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_Config, _Org_Config, value);
    				this._Org_Config = value;
    			}
    		}
    		
    		public Int32 Org_CourseCount {
    			get {
    				return this._Org_CourseCount;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_CourseCount, _Org_CourseCount, value);
    				this._Org_CourseCount = value;
    			}
    		}
    		
    		public String Org_Description {
    			get {
    				return this._Org_Description;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_Description, _Org_Description, value);
    				this._Org_Description = value;
    			}
    		}
    		
    		public String Org_District {
    			get {
    				return this._Org_District;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_District, _Org_District, value);
    				this._Org_District = value;
    			}
    		}
    		
    		public String Org_Email {
    			get {
    				return this._Org_Email;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_Email, _Org_Email, value);
    				this._Org_Email = value;
    			}
    		}
    		
    		public String Org_EnName {
    			get {
    				return this._Org_EnName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_EnName, _Org_EnName, value);
    				this._Org_EnName = value;
    			}
    		}
    		
    		public String Org_ExtraMobi {
    			get {
    				return this._Org_ExtraMobi;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_ExtraMobi, _Org_ExtraMobi, value);
    				this._Org_ExtraMobi = value;
    			}
    		}
    		
    		public String Org_ExtraWeb {
    			get {
    				return this._Org_ExtraWeb;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_ExtraWeb, _Org_ExtraWeb, value);
    				this._Org_ExtraWeb = value;
    			}
    		}
    		
    		public String Org_Fax {
    			get {
    				return this._Org_Fax;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_Fax, _Org_Fax, value);
    				this._Org_Fax = value;
    			}
    		}
    		
    		public String Org_GonganBeian {
    			get {
    				return this._Org_GonganBeian;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_GonganBeian, _Org_GonganBeian, value);
    				this._Org_GonganBeian = value;
    			}
    		}
    		
    		public String Org_ICP {
    			get {
    				return this._Org_ICP;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_ICP, _Org_ICP, value);
    				this._Org_ICP = value;
    			}
    		}
    		
    		public String Org_Intro {
    			get {
    				return this._Org_Intro;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_Intro, _Org_Intro, value);
    				this._Org_Intro = value;
    			}
    		}
    		
    		public Boolean Org_IsDefault {
    			get {
    				return this._Org_IsDefault;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_IsDefault, _Org_IsDefault, value);
    				this._Org_IsDefault = value;
    			}
    		}
    		
    		public Boolean Org_IsPass {
    			get {
    				return this._Org_IsPass;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_IsPass, _Org_IsPass, value);
    				this._Org_IsPass = value;
    			}
    		}
    		
    		public Boolean Org_IsRoot {
    			get {
    				return this._Org_IsRoot;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_IsRoot, _Org_IsRoot, value);
    				this._Org_IsRoot = value;
    			}
    		}
    		
    		public Boolean Org_IsShow {
    			get {
    				return this._Org_IsShow;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_IsShow, _Org_IsShow, value);
    				this._Org_IsShow = value;
    			}
    		}
    		
    		public Boolean Org_IsUse {
    			get {
    				return this._Org_IsUse;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_IsUse, _Org_IsUse, value);
    				this._Org_IsUse = value;
    			}
    		}
    		
    		public String Org_Keywords {
    			get {
    				return this._Org_Keywords;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_Keywords, _Org_Keywords, value);
    				this._Org_Keywords = value;
    			}
    		}
    		
    		public String Org_Lang {
    			get {
    				return this._Org_Lang;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_Lang, _Org_Lang, value);
    				this._Org_Lang = value;
    			}
    		}
    		
    		public String Org_Latitude {
    			get {
    				return this._Org_Latitude;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_Latitude, _Org_Latitude, value);
    				this._Org_Latitude = value;
    			}
    		}
    		
    		public String Org_Linkman {
    			get {
    				return this._Org_Linkman;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_Linkman, _Org_Linkman, value);
    				this._Org_Linkman = value;
    			}
    		}
    		
    		public String Org_LinkmanPhone {
    			get {
    				return this._Org_LinkmanPhone;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_LinkmanPhone, _Org_LinkmanPhone, value);
    				this._Org_LinkmanPhone = value;
    			}
    		}
    		
    		public String Org_LinkmanQQ {
    			get {
    				return this._Org_LinkmanQQ;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_LinkmanQQ, _Org_LinkmanQQ, value);
    				this._Org_LinkmanQQ = value;
    			}
    		}
    		
    		public String Org_Logo {
    			get {
    				return this._Org_Logo;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_Logo, _Org_Logo, value);
    				this._Org_Logo = value;
    			}
    		}
    		
    		public String Org_Longitude {
    			get {
    				return this._Org_Longitude;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_Longitude, _Org_Longitude, value);
    				this._Org_Longitude = value;
    			}
    		}
    		
    		public String Org_Name {
    			get {
    				return this._Org_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_Name, _Org_Name, value);
    				this._Org_Name = value;
    			}
    		}
    		
    		public String Org_Owner {
    			get {
    				return this._Org_Owner;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_Owner, _Org_Owner, value);
    				this._Org_Owner = value;
    			}
    		}
    		
    		public String Org_Phone {
    			get {
    				return this._Org_Phone;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_Phone, _Org_Phone, value);
    				this._Org_Phone = value;
    			}
    		}
    		
    		public String Org_PlatformName {
    			get {
    				return this._Org_PlatformName;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_PlatformName, _Org_PlatformName, value);
    				this._Org_PlatformName = value;
    			}
    		}
    		
    		public String Org_Province {
    			get {
    				return this._Org_Province;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_Province, _Org_Province, value);
    				this._Org_Province = value;
    			}
    		}
    		
    		public Int32 Org_QuesCount {
    			get {
    				return this._Org_QuesCount;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_QuesCount, _Org_QuesCount, value);
    				this._Org_QuesCount = value;
    			}
    		}
    		
    		public DateTime Org_RegTime {
    			get {
    				return this._Org_RegTime;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_RegTime, _Org_RegTime, value);
    				this._Org_RegTime = value;
    			}
    		}
    		
    		public String Org_Street {
    			get {
    				return this._Org_Street;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_Street, _Org_Street, value);
    				this._Org_Street = value;
    			}
    		}
    		
    		public String Org_Template {
    			get {
    				return this._Org_Template;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_Template, _Org_Template, value);
    				this._Org_Template = value;
    			}
    		}
    		
    		public String Org_TemplateMobi {
    			get {
    				return this._Org_TemplateMobi;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_TemplateMobi, _Org_TemplateMobi, value);
    				this._Org_TemplateMobi = value;
    			}
    		}
    		
    		public String Org_TwoDomain {
    			get {
    				return this._Org_TwoDomain;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_TwoDomain, _Org_TwoDomain, value);
    				this._Org_TwoDomain = value;
    			}
    		}
    		
    		public String Org_USCI {
    			get {
    				return this._Org_USCI;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_USCI, _Org_USCI, value);
    				this._Org_USCI = value;
    			}
    		}
    		
    		public String Org_WebSite {
    			get {
    				return this._Org_WebSite;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_WebSite, _Org_WebSite, value);
    				this._Org_WebSite = value;
    			}
    		}
    		
    		public String Org_Weixin {
    			get {
    				return this._Org_Weixin;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_Weixin, _Org_Weixin, value);
    				this._Org_Weixin = value;
    			}
    		}
    		
    		public String Org_Zip {
    			get {
    				return this._Org_Zip;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_Zip, _Org_Zip, value);
    				this._Org_Zip = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<Organization>("Organization");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override WeiSha.Data.Field GetIdentityField() {
    			return _.Org_ID;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Org_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Org_ID,
    					_.Olv_ID,
    					_.Olv_Name,
    					_.Org_AbbrEnName,
    					_.Org_AbbrName,
    					_.Org_Address,
    					_.Org_BankAcc,
    					_.Org_City,
    					_.Org_CoBank,
    					_.Org_Config,
    					_.Org_CourseCount,
    					_.Org_Description,
    					_.Org_District,
    					_.Org_Email,
    					_.Org_EnName,
    					_.Org_ExtraMobi,
    					_.Org_ExtraWeb,
    					_.Org_Fax,
    					_.Org_GonganBeian,
    					_.Org_ICP,
    					_.Org_Intro,
    					_.Org_IsDefault,
    					_.Org_IsPass,
    					_.Org_IsRoot,
    					_.Org_IsShow,
    					_.Org_IsUse,
    					_.Org_Keywords,
    					_.Org_Lang,
    					_.Org_Latitude,
    					_.Org_Linkman,
    					_.Org_LinkmanPhone,
    					_.Org_LinkmanQQ,
    					_.Org_Logo,
    					_.Org_Longitude,
    					_.Org_Name,
    					_.Org_Owner,
    					_.Org_Phone,
    					_.Org_PlatformName,
    					_.Org_Province,
    					_.Org_QuesCount,
    					_.Org_RegTime,
    					_.Org_Street,
    					_.Org_Template,
    					_.Org_TemplateMobi,
    					_.Org_TwoDomain,
    					_.Org_USCI,
    					_.Org_WebSite,
    					_.Org_Weixin,
    					_.Org_Zip};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Org_ID,
    					this._Olv_ID,
    					this._Olv_Name,
    					this._Org_AbbrEnName,
    					this._Org_AbbrName,
    					this._Org_Address,
    					this._Org_BankAcc,
    					this._Org_City,
    					this._Org_CoBank,
    					this._Org_Config,
    					this._Org_CourseCount,
    					this._Org_Description,
    					this._Org_District,
    					this._Org_Email,
    					this._Org_EnName,
    					this._Org_ExtraMobi,
    					this._Org_ExtraWeb,
    					this._Org_Fax,
    					this._Org_GonganBeian,
    					this._Org_ICP,
    					this._Org_Intro,
    					this._Org_IsDefault,
    					this._Org_IsPass,
    					this._Org_IsRoot,
    					this._Org_IsShow,
    					this._Org_IsUse,
    					this._Org_Keywords,
    					this._Org_Lang,
    					this._Org_Latitude,
    					this._Org_Linkman,
    					this._Org_LinkmanPhone,
    					this._Org_LinkmanQQ,
    					this._Org_Logo,
    					this._Org_Longitude,
    					this._Org_Name,
    					this._Org_Owner,
    					this._Org_Phone,
    					this._Org_PlatformName,
    					this._Org_Province,
    					this._Org_QuesCount,
    					this._Org_RegTime,
    					this._Org_Street,
    					this._Org_Template,
    					this._Org_TemplateMobi,
    					this._Org_TwoDomain,
    					this._Org_USCI,
    					this._Org_WebSite,
    					this._Org_Weixin,
    					this._Org_Zip};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Org_ID))) {
    				this._Org_ID = reader.GetInt32(_.Org_ID);
    			}
    			if ((false == reader.IsDBNull(_.Olv_ID))) {
    				this._Olv_ID = reader.GetInt32(_.Olv_ID);
    			}
    			if ((false == reader.IsDBNull(_.Olv_Name))) {
    				this._Olv_Name = reader.GetString(_.Olv_Name);
    			}
    			if ((false == reader.IsDBNull(_.Org_AbbrEnName))) {
    				this._Org_AbbrEnName = reader.GetString(_.Org_AbbrEnName);
    			}
    			if ((false == reader.IsDBNull(_.Org_AbbrName))) {
    				this._Org_AbbrName = reader.GetString(_.Org_AbbrName);
    			}
    			if ((false == reader.IsDBNull(_.Org_Address))) {
    				this._Org_Address = reader.GetString(_.Org_Address);
    			}
    			if ((false == reader.IsDBNull(_.Org_BankAcc))) {
    				this._Org_BankAcc = reader.GetString(_.Org_BankAcc);
    			}
    			if ((false == reader.IsDBNull(_.Org_City))) {
    				this._Org_City = reader.GetString(_.Org_City);
    			}
    			if ((false == reader.IsDBNull(_.Org_CoBank))) {
    				this._Org_CoBank = reader.GetString(_.Org_CoBank);
    			}
    			if ((false == reader.IsDBNull(_.Org_Config))) {
    				this._Org_Config = reader.GetString(_.Org_Config);
    			}
    			if ((false == reader.IsDBNull(_.Org_CourseCount))) {
    				this._Org_CourseCount = reader.GetInt32(_.Org_CourseCount);
    			}
    			if ((false == reader.IsDBNull(_.Org_Description))) {
    				this._Org_Description = reader.GetString(_.Org_Description);
    			}
    			if ((false == reader.IsDBNull(_.Org_District))) {
    				this._Org_District = reader.GetString(_.Org_District);
    			}
    			if ((false == reader.IsDBNull(_.Org_Email))) {
    				this._Org_Email = reader.GetString(_.Org_Email);
    			}
    			if ((false == reader.IsDBNull(_.Org_EnName))) {
    				this._Org_EnName = reader.GetString(_.Org_EnName);
    			}
    			if ((false == reader.IsDBNull(_.Org_ExtraMobi))) {
    				this._Org_ExtraMobi = reader.GetString(_.Org_ExtraMobi);
    			}
    			if ((false == reader.IsDBNull(_.Org_ExtraWeb))) {
    				this._Org_ExtraWeb = reader.GetString(_.Org_ExtraWeb);
    			}
    			if ((false == reader.IsDBNull(_.Org_Fax))) {
    				this._Org_Fax = reader.GetString(_.Org_Fax);
    			}
    			if ((false == reader.IsDBNull(_.Org_GonganBeian))) {
    				this._Org_GonganBeian = reader.GetString(_.Org_GonganBeian);
    			}
    			if ((false == reader.IsDBNull(_.Org_ICP))) {
    				this._Org_ICP = reader.GetString(_.Org_ICP);
    			}
    			if ((false == reader.IsDBNull(_.Org_Intro))) {
    				this._Org_Intro = reader.GetString(_.Org_Intro);
    			}
    			if ((false == reader.IsDBNull(_.Org_IsDefault))) {
    				this._Org_IsDefault = reader.GetBoolean(_.Org_IsDefault);
    			}
    			if ((false == reader.IsDBNull(_.Org_IsPass))) {
    				this._Org_IsPass = reader.GetBoolean(_.Org_IsPass);
    			}
    			if ((false == reader.IsDBNull(_.Org_IsRoot))) {
    				this._Org_IsRoot = reader.GetBoolean(_.Org_IsRoot);
    			}
    			if ((false == reader.IsDBNull(_.Org_IsShow))) {
    				this._Org_IsShow = reader.GetBoolean(_.Org_IsShow);
    			}
    			if ((false == reader.IsDBNull(_.Org_IsUse))) {
    				this._Org_IsUse = reader.GetBoolean(_.Org_IsUse);
    			}
    			if ((false == reader.IsDBNull(_.Org_Keywords))) {
    				this._Org_Keywords = reader.GetString(_.Org_Keywords);
    			}
    			if ((false == reader.IsDBNull(_.Org_Lang))) {
    				this._Org_Lang = reader.GetString(_.Org_Lang);
    			}
    			if ((false == reader.IsDBNull(_.Org_Latitude))) {
    				this._Org_Latitude = reader.GetString(_.Org_Latitude);
    			}
    			if ((false == reader.IsDBNull(_.Org_Linkman))) {
    				this._Org_Linkman = reader.GetString(_.Org_Linkman);
    			}
    			if ((false == reader.IsDBNull(_.Org_LinkmanPhone))) {
    				this._Org_LinkmanPhone = reader.GetString(_.Org_LinkmanPhone);
    			}
    			if ((false == reader.IsDBNull(_.Org_LinkmanQQ))) {
    				this._Org_LinkmanQQ = reader.GetString(_.Org_LinkmanQQ);
    			}
    			if ((false == reader.IsDBNull(_.Org_Logo))) {
    				this._Org_Logo = reader.GetString(_.Org_Logo);
    			}
    			if ((false == reader.IsDBNull(_.Org_Longitude))) {
    				this._Org_Longitude = reader.GetString(_.Org_Longitude);
    			}
    			if ((false == reader.IsDBNull(_.Org_Name))) {
    				this._Org_Name = reader.GetString(_.Org_Name);
    			}
    			if ((false == reader.IsDBNull(_.Org_Owner))) {
    				this._Org_Owner = reader.GetString(_.Org_Owner);
    			}
    			if ((false == reader.IsDBNull(_.Org_Phone))) {
    				this._Org_Phone = reader.GetString(_.Org_Phone);
    			}
    			if ((false == reader.IsDBNull(_.Org_PlatformName))) {
    				this._Org_PlatformName = reader.GetString(_.Org_PlatformName);
    			}
    			if ((false == reader.IsDBNull(_.Org_Province))) {
    				this._Org_Province = reader.GetString(_.Org_Province);
    			}
    			if ((false == reader.IsDBNull(_.Org_QuesCount))) {
    				this._Org_QuesCount = reader.GetInt32(_.Org_QuesCount);
    			}
    			if ((false == reader.IsDBNull(_.Org_RegTime))) {
    				this._Org_RegTime = reader.GetDateTime(_.Org_RegTime);
    			}
    			if ((false == reader.IsDBNull(_.Org_Street))) {
    				this._Org_Street = reader.GetString(_.Org_Street);
    			}
    			if ((false == reader.IsDBNull(_.Org_Template))) {
    				this._Org_Template = reader.GetString(_.Org_Template);
    			}
    			if ((false == reader.IsDBNull(_.Org_TemplateMobi))) {
    				this._Org_TemplateMobi = reader.GetString(_.Org_TemplateMobi);
    			}
    			if ((false == reader.IsDBNull(_.Org_TwoDomain))) {
    				this._Org_TwoDomain = reader.GetString(_.Org_TwoDomain);
    			}
    			if ((false == reader.IsDBNull(_.Org_USCI))) {
    				this._Org_USCI = reader.GetString(_.Org_USCI);
    			}
    			if ((false == reader.IsDBNull(_.Org_WebSite))) {
    				this._Org_WebSite = reader.GetString(_.Org_WebSite);
    			}
    			if ((false == reader.IsDBNull(_.Org_Weixin))) {
    				this._Org_Weixin = reader.GetString(_.Org_Weixin);
    			}
    			if ((false == reader.IsDBNull(_.Org_Zip))) {
    				this._Org_Zip = reader.GetString(_.Org_Zip);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(Organization).IsAssignableFrom(obj.GetType()))) {
    				return false;
    			}
    			if ((((object)(this)) == ((object)(obj)))) {
    				return true;
    			}
    			return false;
    		}
    		
    		public class _ {
    			
    			/// <summary>
    			/// 表示选择所有列，与*等同
    			/// </summary>
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Organization>();
    			
    			/// <summary>
    			/// 字段名：Org_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_ID = new WeiSha.Data.Field<Organization>("Org_ID");
    			
    			/// <summary>
    			/// 字段名：Olv_ID - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Olv_ID = new WeiSha.Data.Field<Organization>("Olv_ID");
    			
    			/// <summary>
    			/// 字段名：Olv_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Olv_Name = new WeiSha.Data.Field<Organization>("Olv_Name");
    			
    			/// <summary>
    			/// 字段名：Org_AbbrEnName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_AbbrEnName = new WeiSha.Data.Field<Organization>("Org_AbbrEnName");
    			
    			/// <summary>
    			/// 字段名：Org_AbbrName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_AbbrName = new WeiSha.Data.Field<Organization>("Org_AbbrName");
    			
    			/// <summary>
    			/// 字段名：Org_Address - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Address = new WeiSha.Data.Field<Organization>("Org_Address");
    			
    			/// <summary>
    			/// 字段名：Org_BankAcc - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_BankAcc = new WeiSha.Data.Field<Organization>("Org_BankAcc");
    			
    			/// <summary>
    			/// 字段名：Org_City - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_City = new WeiSha.Data.Field<Organization>("Org_City");
    			
    			/// <summary>
    			/// 字段名：Org_CoBank - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_CoBank = new WeiSha.Data.Field<Organization>("Org_CoBank");
    			
    			/// <summary>
    			/// 字段名：Org_Config - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Config = new WeiSha.Data.Field<Organization>("Org_Config");
    			
    			/// <summary>
    			/// 字段名：Org_CourseCount - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_CourseCount = new WeiSha.Data.Field<Organization>("Org_CourseCount");
    			
    			/// <summary>
    			/// 字段名：Org_Description - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Description = new WeiSha.Data.Field<Organization>("Org_Description");
    			
    			/// <summary>
    			/// 字段名：Org_District - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_District = new WeiSha.Data.Field<Organization>("Org_District");
    			
    			/// <summary>
    			/// 字段名：Org_Email - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Email = new WeiSha.Data.Field<Organization>("Org_Email");
    			
    			/// <summary>
    			/// 字段名：Org_EnName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_EnName = new WeiSha.Data.Field<Organization>("Org_EnName");
    			
    			/// <summary>
    			/// 字段名：Org_ExtraMobi - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_ExtraMobi = new WeiSha.Data.Field<Organization>("Org_ExtraMobi");
    			
    			/// <summary>
    			/// 字段名：Org_ExtraWeb - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_ExtraWeb = new WeiSha.Data.Field<Organization>("Org_ExtraWeb");
    			
    			/// <summary>
    			/// 字段名：Org_Fax - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Fax = new WeiSha.Data.Field<Organization>("Org_Fax");
    			
    			/// <summary>
    			/// 字段名：Org_GonganBeian - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_GonganBeian = new WeiSha.Data.Field<Organization>("Org_GonganBeian");
    			
    			/// <summary>
    			/// 字段名：Org_ICP - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_ICP = new WeiSha.Data.Field<Organization>("Org_ICP");
    			
    			/// <summary>
    			/// 字段名：Org_Intro - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Intro = new WeiSha.Data.Field<Organization>("Org_Intro");
    			
    			/// <summary>
    			/// 字段名：Org_IsDefault - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Org_IsDefault = new WeiSha.Data.Field<Organization>("Org_IsDefault");
    			
    			/// <summary>
    			/// 字段名：Org_IsPass - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Org_IsPass = new WeiSha.Data.Field<Organization>("Org_IsPass");
    			
    			/// <summary>
    			/// 字段名：Org_IsRoot - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Org_IsRoot = new WeiSha.Data.Field<Organization>("Org_IsRoot");
    			
    			/// <summary>
    			/// 字段名：Org_IsShow - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Org_IsShow = new WeiSha.Data.Field<Organization>("Org_IsShow");
    			
    			/// <summary>
    			/// 字段名：Org_IsUse - 数据类型：Boolean
    			/// </summary>
    			public static WeiSha.Data.Field Org_IsUse = new WeiSha.Data.Field<Organization>("Org_IsUse");
    			
    			/// <summary>
    			/// 字段名：Org_Keywords - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Keywords = new WeiSha.Data.Field<Organization>("Org_Keywords");
    			
    			/// <summary>
    			/// 字段名：Org_Lang - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Lang = new WeiSha.Data.Field<Organization>("Org_Lang");
    			
    			/// <summary>
    			/// 字段名：Org_Latitude - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Latitude = new WeiSha.Data.Field<Organization>("Org_Latitude");
    			
    			/// <summary>
    			/// 字段名：Org_Linkman - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Linkman = new WeiSha.Data.Field<Organization>("Org_Linkman");
    			
    			/// <summary>
    			/// 字段名：Org_LinkmanPhone - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_LinkmanPhone = new WeiSha.Data.Field<Organization>("Org_LinkmanPhone");
    			
    			/// <summary>
    			/// 字段名：Org_LinkmanQQ - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_LinkmanQQ = new WeiSha.Data.Field<Organization>("Org_LinkmanQQ");
    			
    			/// <summary>
    			/// 字段名：Org_Logo - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Logo = new WeiSha.Data.Field<Organization>("Org_Logo");
    			
    			/// <summary>
    			/// 字段名：Org_Longitude - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Longitude = new WeiSha.Data.Field<Organization>("Org_Longitude");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Name = new WeiSha.Data.Field<Organization>("Org_Name");
    			
    			/// <summary>
    			/// 字段名：Org_Owner - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Owner = new WeiSha.Data.Field<Organization>("Org_Owner");
    			
    			/// <summary>
    			/// 字段名：Org_Phone - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Phone = new WeiSha.Data.Field<Organization>("Org_Phone");
    			
    			/// <summary>
    			/// 字段名：Org_PlatformName - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_PlatformName = new WeiSha.Data.Field<Organization>("Org_PlatformName");
    			
    			/// <summary>
    			/// 字段名：Org_Province - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Province = new WeiSha.Data.Field<Organization>("Org_Province");
    			
    			/// <summary>
    			/// 字段名：Org_QuesCount - 数据类型：Int32
    			/// </summary>
    			public static WeiSha.Data.Field Org_QuesCount = new WeiSha.Data.Field<Organization>("Org_QuesCount");
    			
    			/// <summary>
    			/// 字段名：Org_RegTime - 数据类型：DateTime
    			/// </summary>
    			public static WeiSha.Data.Field Org_RegTime = new WeiSha.Data.Field<Organization>("Org_RegTime");
    			
    			/// <summary>
    			/// 字段名：Org_Street - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Street = new WeiSha.Data.Field<Organization>("Org_Street");
    			
    			/// <summary>
    			/// 字段名：Org_Template - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Template = new WeiSha.Data.Field<Organization>("Org_Template");
    			
    			/// <summary>
    			/// 字段名：Org_TemplateMobi - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_TemplateMobi = new WeiSha.Data.Field<Organization>("Org_TemplateMobi");
    			
    			/// <summary>
    			/// 字段名：Org_TwoDomain - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_TwoDomain = new WeiSha.Data.Field<Organization>("Org_TwoDomain");
    			
    			/// <summary>
    			/// 字段名：Org_USCI - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_USCI = new WeiSha.Data.Field<Organization>("Org_USCI");
    			
    			/// <summary>
    			/// 字段名：Org_WebSite - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_WebSite = new WeiSha.Data.Field<Organization>("Org_WebSite");
    			
    			/// <summary>
    			/// 字段名：Org_Weixin - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Weixin = new WeiSha.Data.Field<Organization>("Org_Weixin");
    			
    			/// <summary>
    			/// 字段名：Org_Zip - 数据类型：String
    			/// </summary>
    			public static WeiSha.Data.Field Org_Zip = new WeiSha.Data.Field<Organization>("Org_Zip");
    		}
    	}
    }
    