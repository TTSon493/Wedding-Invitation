
import { useState } from "react";
import { motion, AnimatePresence } from "framer-motion";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { Card, CardContent } from "@/components/ui/card";
import { Slider } from "@/components/ui/slider";
import { Switch } from "@/components/ui/switch";
import { ChevronDown, ChevronUp, Heart, Calendar, MapPin } from "lucide-react";

const backgroundImages = [
  "/placeholder.svg?height=600&width=400&text=Floral",
  "/placeholder.svg?height=600&width=400&text=Beach",
  "/placeholder.svg?height=600&width=400&text=Mountain",
  "/placeholder.svg?height=600&width=400&text=City",
];

export default function TemplateSingle() {
  const [invitationData, setInvitationData] = useState({
    coupleNames: "Emma & Liam",
    date: "August 15, 2024",
    time: "4:00 PM",
    venue: "Rosewood Garden",
    address: "123 Blossom Street, Springville",
    message: "Join us in celebrating our love story!",
  });

  const [design, setDesign] = useState({
    backgroundIndex: 0,
    textColor: "#ffffff",
    fontSize: 16,
    showAnimation: true,
  });

  const [isEditing, setIsEditing] = useState(false);

  const handleInputChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    const { name, value } = e.target;
    setInvitationData((prev) => ({ ...prev, [name]: value }));
  };

  const handleDesignChange = (key: string, value: any) => {
    setDesign((prev) => ({ ...prev, [key]: value }));
  };

  return (
    <div className='min-h-screen bg-gray-100 py-12 px-4 sm:px-6 lg:px-8'>
      <div className='max-w-3xl mx-auto'>
        <h1 className='text-3xl font-bold text-center mb-8'>
          Premium Wedding Invitation
        </h1>
        <div className='flex flex-row gap-8'>
          <Card className='flex-1'>
            <CardContent className='p-0 overflow-hidden w-[500px]'>
              <div
                className='relative h-[600px] bg-cover bg-center flex items-center justify-center'
                style={{
                  backgroundImage: `url(${
                    backgroundImages[design.backgroundIndex]
                  })`,
                }}>
                <div className='absolute inset-0 bg-black bg-opacity-40'></div>
                <AnimatePresence>
                  {design.showAnimation && (
                    <motion.div
                      initial={{ opacity: 0, y: 20 }}
                      animate={{ opacity: 1, y: 0 }}
                      exit={{ opacity: 0, y: -20 }}
                      transition={{ duration: 0.5 }}
                      className='relative z-10 text-center p-8'
                      style={{
                        color: design.textColor,
                        fontSize: `${design.fontSize}px`,
                      }}>
                      <motion.div
                        initial={{ scale: 0 }}
                        animate={{ scale: 1 }}
                        transition={{
                          delay: 0.3,
                          type: "spring",
                          stiffness: 260,
                          damping: 20,
                        }}>
                        <Heart className='w-16 h-16 mx-auto mb-4' />
                      </motion.div>
                      <h2 className='font-script text-4xl mb-4'>
                        {invitationData.coupleNames}
                      </h2>
                      <p className='mb-6'>{invitationData.message}</p>
                      <div className='flex justify-center items-center mb-4'>
                        <Calendar className='w-5 h-5 mr-2' />
                        <p>
                          {invitationData.date} at {invitationData.time}
                        </p>
                      </div>
                      <div className='flex justify-center items-center'>
                        <MapPin className='w-5 h-5 mr-2' />
                        <p>
                          {invitationData.venue}, {invitationData.address}
                        </p>
                      </div>
                    </motion.div>
                  )}
                </AnimatePresence>
              </div>
            </CardContent>
          </Card>
          <Card className='flex-1'>
            <CardContent className='w-[500px]'>
              <div className='space-y-4'>
                <Button
                  onClick={() => setIsEditing(!isEditing)}
                  className='w-full'>
                  {isEditing ? (
                    <>
                      <ChevronUp className='w-4 h-4 mr-2' />
                      Hide Editor
                    </>
                  ) : (
                    <>
                      <ChevronDown className='w-4 h-4 mr-2' />
                      Customize Invitation
                    </>
                  )}
                </Button>
                <AnimatePresence>
                  {isEditing && (
                    <motion.div
                      initial={{ opacity: 0, height: 0 }}
                      animate={{ opacity: 1, height: "auto" }}
                      exit={{ opacity: 0, height: 0 }}
                      transition={{ duration: 0.3 }}
                      className='space-y-4 overflow-hidden'>
                      <div>
                        <Label htmlFor='coupleNames'>Couple Names</Label>
                        <Input
                          id='coupleNames'
                          name='coupleNames'
                          value={invitationData.coupleNames}
                          onChange={handleInputChange}
                        />
                      </div>
                      <div>
                        <Label htmlFor='date'>Date</Label>
                        <Input
                          id='date'
                          name='date'
                          value={invitationData.date}
                          onChange={handleInputChange}
                        />
                      </div>
                      <div>
                        <Label htmlFor='time'>Time</Label>
                        <Input
                          id='time'
                          name='time'
                          value={invitationData.time}
                          onChange={handleInputChange}
                        />
                      </div>
                      <div>
                        <Label htmlFor='venue'>Venue</Label>
                        <Input
                          id='venue'
                          name='venue'
                          value={invitationData.venue}
                          onChange={handleInputChange}
                        />
                      </div>
                      <div>
                        <Label htmlFor='address'>Address</Label>
                        <Input
                          id='address'
                          name='address'
                          value={invitationData.address}
                          onChange={handleInputChange}
                        />
                      </div>
                      <div>
                        <Label htmlFor='message'>Message</Label>
                        <Textarea
                          id='message'
                          name='message'
                          value={invitationData.message}
                          onChange={handleInputChange}
                        />
                      </div>
                      <div>
                        <Label>Background Image</Label>
                        <div className='flex space-x-2 mt-2'>
                          {backgroundImages.map((_, index) => (
                            <Button
                              key={index}
                              variant={
                                design.backgroundIndex === index
                                  ? "default"
                                  : "outline"
                              }
                              className='w-8 h-8 p-0'
                              onClick={() =>
                                handleDesignChange("backgroundIndex", index)
                              }>
                              {index + 1}
                            </Button>
                          ))}
                        </div>
                      </div>
                      <div>
                        <Label htmlFor='textColor'>Text Color</Label>
                        <Input
                          id='textColor'
                          type='color'
                          value={design.textColor}
                          onChange={(e) =>
                            handleDesignChange("textColor", e.target.value)
                          }
                          className='h-10 p-1'
                        />
                      </div>
                      <div>
                        <Label htmlFor='fontSize'>Font Size</Label>
                        <Slider
                          id='fontSize'
                          min={12}
                          max={24}
                          step={1}
                          value={[design.fontSize]}
                          onValueChange={(value) =>
                            handleDesignChange("fontSize", value[0])
                          }
                          className='mt-2'
                        />
                      </div>
                      <div className='flex items-center space-x-2'>
                        <Switch
                          id='showAnimation'
                          checked={design.showAnimation}
                          onCheckedChange={(checked) =>
                            handleDesignChange("showAnimation", checked)
                          }
                        />
                        <Label htmlFor='showAnimation'>Show Animation</Label>
                      </div>
                    </motion.div>
                  )}
                </AnimatePresence>
              </div>
            </CardContent>
          </Card>
        </div>
      </div>
    </div>
  );
}
