import { useState } from "react";
import VerticalMenu from "@/components/VerticalMenu";
import ProductDetails from "@/components/ProductDetails";
import ProductImage from "@/components/ProductImage";
import RelatedProducts from "@/components/RelatedProducts";

export default function InvitationCard() {
  const [activeComponent, setActiveComponent] = useState<string>("card");

  return (
    <div className='container mx-auto px-4 py-8'>
      {/* <Breadcrumb /> */}
      <div className='grid md:grid-cols-[auto,1fr,1fr] gap-8 mt-20'>
        <VerticalMenu setActiveComponent={setActiveComponent} />
        <ProductImage />
        <ProductDetails activeComponent={activeComponent} />
      </div>
      <RelatedProducts />
    </div>
  );
}
