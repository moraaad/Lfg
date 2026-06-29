import { Navbar } from "./components/Navbar";
import { Hero } from "./components/Hero";
import { SplitSection } from "./components/SplitSection";
import { FeaturedProducts } from "./components/FeaturedProducts";
import { BrandStatement } from "./components/BrandStatement";
import { Newsletter } from "./components/Newsletter";
import { Footer } from "./components/Footer";

export default function App() {
    return (
        <div style={{ backgroundColor: "#F5F0E8" }} className="min-h-screen">
            <Navbar />
            <main>
                <Hero />
                <SplitSection />
                <FeaturedProducts />
                <BrandStatement />
                <Newsletter />
            </main>
            <Footer />
        </div>
    );
}
