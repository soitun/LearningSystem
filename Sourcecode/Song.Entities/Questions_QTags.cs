namespace Song.Entities {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：Questions_QTags 主键列：Qqt_ID
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class Questions_QTags : WeiSha.Data.Entity {
    		
    		protected Int64 _Qqt_ID;
    		
    		protected Int64 _Qtag_ID;
    		
    		protected Int64 _Ques_ID;
    		
    		public Int64 Qqt_ID {
    			get {
    				return this._Qqt_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qqt_ID, _Qqt_ID, value);
    				this._Qqt_ID = value;
    			}
    		}
    		
    		public Int64 Qtag_ID {
    			get {
    				return this._Qtag_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Qtag_ID, _Qtag_ID, value);
    				this._Qtag_ID = value;
    			}
    		}
    		
    		public Int64 Ques_ID {
    			get {
    				return this._Ques_ID;
    			}
    			set {
    				this.OnPropertyValueChange(_.Ques_ID, _Ques_ID, value);
    				this._Ques_ID = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override WeiSha.Data.Table GetTable() {
    			return new WeiSha.Data.Table<Questions_QTags>("Questions_QTags");
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetPrimaryKeyFields() {
    			return new WeiSha.Data.Field[] {
    					_.Qqt_ID};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override WeiSha.Data.Field[] GetFields() {
    			return new WeiSha.Data.Field[] {
    					_.Qqt_ID,
    					_.Qtag_ID,
    					_.Ques_ID};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Qqt_ID,
    					this._Qtag_ID,
    					this._Ques_ID};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(WeiSha.Data.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Qqt_ID))) {
    				this._Qqt_ID = reader.GetInt64(_.Qqt_ID);
    			}
    			if ((false == reader.IsDBNull(_.Qtag_ID))) {
    				this._Qtag_ID = reader.GetInt64(_.Qtag_ID);
    			}
    			if ((false == reader.IsDBNull(_.Ques_ID))) {
    				this._Ques_ID = reader.GetInt64(_.Ques_ID);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(Questions_QTags).IsAssignableFrom(obj.GetType()))) {
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
    			public static WeiSha.Data.AllField All = new WeiSha.Data.AllField<Questions_QTags>();
    			
    			/// <summary>
    			/// 字段名：Qqt_ID - 数据类型：Int64
    			/// </summary>
    			public static WeiSha.Data.Field Qqt_ID = new WeiSha.Data.Field<Questions_QTags>("Qqt_ID");
    			
    			/// <summary>
    			/// 字段名：Qtag_ID - 数据类型：Int64
    			/// </summary>
    			public static WeiSha.Data.Field Qtag_ID = new WeiSha.Data.Field<Questions_QTags>("Qtag_ID");
    			
    			/// <summary>
    			/// 字段名：Ques_ID - 数据类型：Int64
    			/// </summary>
    			public static WeiSha.Data.Field Ques_ID = new WeiSha.Data.Field<Questions_QTags>("Ques_ID");
    		}
    	}
    }
    