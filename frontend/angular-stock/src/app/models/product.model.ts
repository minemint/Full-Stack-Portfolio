export class ProductModel {
    products!: []
    productid!: number
    productname!: string
    unitprice!: number
    unitinstock!: number
    productpicture!: string
    categoryid!: number
    categoryname!: string
    createddate!: Date
    modifieddate!: Date
}

export class ProductCreateUpdateModel {
    productname!: string
    unitprice!: number
    unitinstock!: number
    productpicture?: string
    categoryid!: number
    categoryname?: string
    createddate!: Date
    modifieddate!: Date
    image?: string
}