using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleAPI.Data.Model;

namespace SampleAPI.Data.EntityConfigurations
{
    public class TestEntityConf : IEntityTypeConfiguration<TestEntity>
    {
        public void Configure(EntityTypeBuilder<TestEntity> builder)
        {
            
        }
    }
}
