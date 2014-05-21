namespace Tim.Cquential.Core

    type IQueryEngine<'T> =
        abstract member Execute : IQuery<'T> -> IMatch<'T>[]

