import Features from "../../components/Features";
import Hero from "../../components/Hero";
import Pricing from "./../../components/Pricing/Pricing";
import HowItWorks from "./../../components/HowItWorks/index";
import CallToAction from "../../components/CallToAction";
// import useAuth from "@/hooks/useAuth";

const Home = () => {
  // const { user } = useAuth();
  // console.log(user);
  return (
    <div>
      <Hero />
      <Features />
      <Pricing />
      <HowItWorks />
      <CallToAction />
    </div>
  );
};

export default Home;
