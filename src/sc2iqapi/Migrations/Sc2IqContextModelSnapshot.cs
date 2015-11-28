using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Relational.Migrations.Infrastructure;
using sc2iqapi.Models;

namespace sc2iqapi.Migrations
{
    [ContextType(typeof(Sc2IqContext))]
    partial class Sc2IqContextModelSnapshot : ModelSnapshot
    {
        public override void BuildModel(ModelBuilder builder)
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
                    
                    b.Property<int>("CreatedBy");
                    
                    b.Property<int>("Difficulty");
                    
                    b.Property<string>("Q");
                    
                    b.Property<int>("State");
                    
                    b.Key("Id");
                });
        }
    }
}
