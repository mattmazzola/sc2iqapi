using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Relational.Migrations.Infrastructure;
using sc2iqapi.Models;

namespace sc2iqapi.Migrations
{
    [ContextType(typeof(Sc2IqContext))]
    partial class AddUser
    {
        public override string Id
        {
            get { return "20151129060412_AddUser"; }
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
                    
                    b.Property<int>("CreatedBy");
                    
                    b.Property<int>("Difficulty");
                    
                    b.Property<string>("Q");
                    
                    b.Property<int>("State");
                    
                    b.Key("Id");
                });
            
            builder.Entity("sc2iqapi.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .GenerateValueOnAdd()
                        .StoreGeneratedPattern(StoreGeneratedPattern.Identity);
                    
                    b.Property<DateTimeOffset>("Created");
                    
                    b.Property<int>("CreatedBy");
                    
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
        }
    }
}
