import { Router } from "express";
import { AppDataSource } from "../index";
import { Product } from "../entities/Product";
import { Like } from "typeorm";

const productRouter = Router();

productRouter.get("/", async (req, res) => {
    const productRepo = AppDataSource.getRepository(Product);
    const products = await productRepo.find();
    res.json(products);
});

productRouter.get("/search", async (req, res) => {
    const productRepo = AppDataSource.getRepository(Product);
    const products = await productRepo.find({
        where: {
            title: Like(`%${req.query.q}%`),
        },
    });
    res.json(products);
});

export default productRouter;
