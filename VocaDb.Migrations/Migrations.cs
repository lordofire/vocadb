﻿using System.Data;
using FluentMigrator;

namespace VocaDb.Migrations {

	[Migration(201501271800)]
	public class AddDiscussionFolders : AutoReversingMigration {

		public override void Up() {
			
			var schema = "discussions";

			if (Schema.Schema(schema).Table("DiscussionFolders").Exists())
				return;

			Create.Schema(schema);

			Create.Table("DiscussionFolders").InSchema(schema)
				.WithColumn("[Id]").AsInt32().PrimaryKey("PK_DiscussionFolders").Identity().NotNullable()
				.WithColumn("Deleted").AsBoolean().NotNullable().WithDefaultValue(false)
				.WithColumn("Description").AsString(int.MaxValue).NotNullable().WithDefaultValue(string.Empty)
				.WithColumn("Name").AsString(200).NotNullable()
				.WithColumn("SortIndex").AsInt32().NotNullable().WithDefaultValue(0);

			Create.Table("DiscussionTopics").InSchema(schema)
				.WithColumn("[Id]").AsInt32().PrimaryKey("PK_DiscussionTopics").Identity().NotNullable()
				.WithColumn("Content").AsString(int.MaxValue).NotNullable()
				.WithColumn("[Created]").AsDateTime().NotNullable()
				.WithColumn("Deleted").AsBoolean().NotNullable().WithDefaultValue(false)
				.WithColumn("Locked").AsBoolean().NotNullable().WithDefaultValue(false)
				.WithColumn("Pinned").AsBoolean().NotNullable().WithDefaultValue(false)
				.WithColumn("Name").AsString(200).NotNullable()
				.WithColumn("[Author]").AsInt32().NotNullable().ForeignKey("FK_DiscussionTopics_Users", "dbo", "[Users]", "[Id]").OnDelete(Rule.None)
				.WithColumn("[Folder]").AsInt32().NotNullable().ForeignKey("FK_DiscussionTopics_DiscussionFolders", schema, "[DiscussionFolders]", "[Id]").OnDelete(Rule.Cascade);

			Create.Table("DiscussionComments").InSchema(schema)
				.WithColumn("[Id]").AsInt32().PrimaryKey("PK_DiscussionComments").Identity().NotNullable()
				.WithColumn("AuthorName").AsString(100).NotNullable()
				.WithColumn("[Created]").AsDateTime().NotNullable()
				.WithColumn("Message").AsString(int.MaxValue).NotNullable()
				.WithColumn("[Author]").AsInt32().Nullable().ForeignKey("FK_DiscussionComments_Users", "dbo", "[Users]", "[Id]").OnDelete(Rule.SetNull)
				.WithColumn("[Topic]").AsInt32().NotNullable().ForeignKey("FK_DiscussionComments_DiscussionTopics", schema, "[DiscussionTopics]", "[Id]").OnDelete(Rule.Cascade);

		}

	}


	/// <summary>
	/// #11 add English translated description field.
	/// </summary>
	[Migration(201501192000)]
	public class AddEnglishTranslatedDescriptions : AutoReversingMigration {

		public override void Up() {

			if (Schema.Table("Artists").Column("DescriptionEng").Exists())
				return;

			Create.Column("DescriptionEng").OnTable("Artists").AsString(int.MaxValue).WithDefaultValue(string.Empty);
			Create.Column("DescriptionEng").OnTable("Albums").AsString(int.MaxValue).WithDefaultValue(string.Empty);
			Create.Column("NotesEng").OnTable("Songs").AsString(int.MaxValue).WithDefaultValue(string.Empty);

		}
	}

	/// <summary>
	/// Add ShowChatbox column to UserOptions table
	/// </summary>
	[Migration(201501232300)]
	public class AddShowChatboxForUser : AutoReversingMigration {

		public override void Up() {
			
			if (Schema.Table("UserOptions").Column("ShowChatbox").Exists())
				return;

			Create.Column("ShowChatbox").OnTable("UserOptions").AsBoolean().WithDefaultValue(true);

		}

	}

}
