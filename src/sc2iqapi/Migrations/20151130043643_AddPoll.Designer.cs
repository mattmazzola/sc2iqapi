using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Relational.Migrations.Infrastructure;
using sc2iqapi.Models;

namespace sc2iqapi.Migrations
{
    [ContextType(typeof(Sc2IqContext))]
    partial class AddPoll
    {
        public override string Id
        {
            get { return "20151130043643_AddPoll"; }
        }
        
        public override string ProductVersion
        {
            get { return "7.0.0-beta5-13549"; }
        }
        
        public override void BuildTargetModel(ModelBuilder builder)
        {
            builder
                .Annotation("SqlServer:DefaultSequenceName", "DefaultSequence")
                .Annotation("SqlServer:Sequence:.DefaultSequence", "'DefaultSequence', '', '1', '10', '', '', 'Int64', 'False'")
                .Annotation("SqlServer:ValueGeneration", "Sequence");
            
            builder.Entity("sc2iqapi.Models.Answer", b =>
                {
                    b.Property<int>("Id")
                        .GenerateValueOnAdd()
                        .StoreGeneratedPattern(StoreGeneratedPattern.Identity);
                    
                    b.Property<int>("AnswerIndex");
                    
                    b.Property<int>("Duration");
                    
                    b.Property<int>("Points");
                    
                    b.Property<int>("QuestionId");
                    
                    b.Property<int?>("ScoreId");
                    
                    b.Key("Id");
                });
            
            builder.Entity("sc2iqapi.Models.Poll", b =>
                {
                    b.Property<int>("Id")
                        .GenerateValueOnAdd()
                        .StoreGeneratedPattern(StoreGeneratedPattern.Identity);
                    
                    b.Property<DateTimeOffset>("Created");
                    
                    b.Property<int?>("CreatedById");
                    
                    b.Property<string>("Description");
                    
                    b.Property<string>("Title");
                    
                    b.Property<int>("Votes");
                    
                    b.Key("Id");
                });
            
            builder.Entity("sc2iqapi.Models.Question", b =>
                {
                    b.Property<int>("Id")
                        .GenerateValueOnAdd()
                        .StoreGeneratedPattern(StoreGeneratedPattern.Identity);
                    
                    b.Property<string>("A1");
                    
                    b.Property<string>("A2");
                    
                    b.Property<string>("A3");
                    
                    b.Property<string>("A4");
                    
                    b.Property<int>("CorrectAnswerIndex");
                    
                    b.Property<DateTimeOffset>("Created");
                    
                    b.Property<int?>("CreatedById");
                    
                    b.Property<int>("Difficulty");
                    
                    b.Property<string>("Q");
                    
                    b.Property<int>("State");
                    
                    b.Key("Id");
                });
            
            builder.Entity("sc2iqapi.Models.QuestionTag", b =>
                {
                    b.Property<int>("QuestionId");
                    
                    b.Property<int>("TagId");
                    
                    b.Key("QuestionId", "TagId");
                });
            
            builder.Entity("sc2iqapi.Models.Score", b =>
                {
                    b.Property<int>("Id")
                        .GenerateValueOnAdd()
                        .StoreGeneratedPattern(StoreGeneratedPattern.Identity);
                    
                    b.Property<int>("Duration");
                    
                    b.Property<int>("Points");
                    
                    b.Property<DateTimeOffset>("Submitted");
                    
                    b.Property<int?>("UserId");
                    
                    b.Key("Id");
                });
            
            builder.Entity("sc2iqapi.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .GenerateValueOnAdd()
                        .StoreGeneratedPattern(StoreGeneratedPattern.Identity);
                    
                    b.Property<DateTimeOffset>("Created");
                    
                    b.Property<int?>("CreatedById");
                    
                    b.Property<string>("Text");
                    
                    b.Key("Id");
                });
            
            builder.Entity("sc2iqapi.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .GenerateValueOnAdd()
                        .StoreGeneratedPattern(StoreGeneratedPattern.Identity);
                    
                    b.Property<string>("BattleNetId");
                    
                    b.Property<DateTimeOffset>("Created");
                    
                    b.Property<int>("PointsEarned");
                    
                    b.Property<int>("PointsSpent");
                    
                    b.Property<int>("Reputation");
                    
                    b.Property<int>("Role");
                    
                    b.Key("Id");
                });
            
            builder.Entity("sc2iqapi.Models.Answer", b =>
                {
                    b.Reference("sc2iqapi.Models.Question")
                        .InverseCollection()
                        .ForeignKey("QuestionId");
                    
                    b.Reference("sc2iqapi.Models.Score")
                        .InverseCollection()
                        .ForeignKey("ScoreId");
                });
            
            builder.Entity("sc2iqapi.Models.Poll", b =>
                {
                    b.Reference("sc2iqapi.Models.User")
                        .InverseCollection()
                        .ForeignKey("CreatedById");
                });
            
            builder.Entity("sc2iqapi.Models.Question", b =>
                {
                    b.Reference("sc2iqapi.Models.User")
                        .InverseCollection()
                        .ForeignKey("CreatedById");
                });
            
            builder.Entity("sc2iqapi.Models.QuestionTag", b =>
                {
                    b.Reference("sc2iqapi.Models.Question")
                        .InverseCollection()
                        .ForeignKey("QuestionId");
                    
                    b.Reference("sc2iqapi.Models.Tag")
                        .InverseCollection()
                        .ForeignKey("TagId");
                });
            
            builder.Entity("sc2iqapi.Models.Score", b =>
                {
                    b.Reference("sc2iqapi.Models.User")
                        .InverseCollection()
                        .ForeignKey("UserId");
                });
            
            builder.Entity("sc2iqapi.Models.Tag", b =>
                {
                    b.Reference("sc2iqapi.Models.User")
                        .InverseCollection()
                        .ForeignKey("CreatedById");
                });
        }
    }
}
