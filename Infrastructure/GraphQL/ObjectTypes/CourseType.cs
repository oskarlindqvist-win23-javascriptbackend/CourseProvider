﻿using Infrastructure.Data.Entities;

namespace Infrastructure.GraphQL.ObjectTypes;

public class CourseType : ObjectType<CourseEntity>
{
    protected override void Configure(IObjectTypeDescriptor<CourseEntity> descriptor)
    {
        descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
        descriptor.Field(c => c.IsBestseller).Type<BooleanType>();
        descriptor.Field(c => c.IsDigital).Type<BooleanType>();
        descriptor.Field(c => c.Categories).Type<ListType<StringType>>();
        descriptor.Field(c => c.Title).Type<StringType>();
        descriptor.Field(c => c.Ingress).Type<StringType>();
        descriptor.Field(c => c.StarRating).Type<DecimalType>();
        descriptor.Field(c => c.LikesInProcent).Type<StringType>();
        descriptor.Field(c => c.Likes).Type<StringType>();
        descriptor.Field(c => c.Hours).Type<StringType>();
        descriptor.Field(c => c.Authors).Type<ListType<AuthorType>>();
        descriptor.Field(c => c.Prices).Type<PricesType>();
        descriptor.Field(c => c.Content).Type<ContentType>();
        descriptor.Field(c => c.ProgramDetails).Type<ProgramDetailItemType>();
    }
}

public class AuthorType : ObjectType<AuthorEntity>
{
    protected override void Configure(IObjectTypeDescriptor<AuthorEntity> descriptor)
    {
        descriptor.Field(a => a.Name).Type<StringType>();
    }
}

public class PricesType : ObjectType<PricesEntity>
{
    protected override void Configure(IObjectTypeDescriptor<PricesEntity> descriptor)
    {
        descriptor.Field(p => p.Currency).Type<StringType>();
        descriptor.Field(p => p.Price).Type<DecimalType>();
        descriptor.Field(p => p.Discount).Type<DecimalType>();
    }
}

public class ContentType : ObjectType<ContentEntity>
{
    protected override void Configure(IObjectTypeDescriptor<ContentEntity> descriptor)
    {
        descriptor.Field(p => p.Description).Type<StringType>();
        descriptor.Field(p => p.Includes).Type<ListType<StringType>>();
        descriptor.Field(p => p.Learn).Type<ListType<StringType>>();
    }
}

public class ProgramDetailItemType : ObjectType<ProgramDetailItemEntity>
{
    protected override void Configure(IObjectTypeDescriptor<ProgramDetailItemEntity> descriptor)
    {
        descriptor.Field(p => p.Title_1).Type<StringType>();
        descriptor.Field(p => p.Description_1).Type<StringType>();
        descriptor.Field(p => p.Title_2).Type<StringType>();
        descriptor.Field(p => p.Description_2).Type<StringType>();
        descriptor.Field(p => p.Title_3).Type<StringType>();
        descriptor.Field(p => p.Description_3).Type<StringType>();
    }
}
