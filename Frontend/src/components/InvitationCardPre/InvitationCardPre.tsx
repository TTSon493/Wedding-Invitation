import { AnimatePresence, motion } from "framer-motion";
import { Heart, Calendar, MapPin } from "lucide-react";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { useState } from "react";

interface InvitationCardPreProps {
  invitationData: {
    coupleNames: string;
    date: string;
    time: string;
    venue: string;
    address: string;
    message: string;
    giftLink?: string;
    musicLink?: string;
  };
  design: {
    backgroundIndex: number;
    textColor: string;
    fontSize: number;
    showAnimation: boolean;
  };
  countdown: string;
  guestView: boolean;
  cardRef: React.RefObject<HTMLDivElement>;
}

export function InvitationCardPre({
  invitationData,
  design,
  countdown,
  cardRef,
}: InvitationCardPreProps) {
  const backgroundImages = [
    "/placeholder.svg?height=800&width=600&text=Floral",
    "/placeholder.svg?height=800&width=600&text=Beach",
    "/placeholder.svg?height=800&width=600&text=Mountain",
    "/placeholder.svg?height=800&width=600&text=City",
  ];

  const [selectedGifts, setSelectedGifts] = useState<string[]>([]);
  const [selectedSongs, setSelectedSongs] = useState<string[]>([]);
  const [giftDialogOpen, setGiftDialogOpen] = useState(false);
  const [songDialogOpen, setSongDialogOpen] = useState(false);

  const giftOptions = [
    "KitchenAid Stand Mixer",
    "Honeymoon Fund Contribution",
    "Dyson Vacuum Cleaner",
    "Set of Le Creuset Cookware",
    "Smart Home Starter Kit",
  ];

  const songOptions = [
    '"Perfect" by Ed Sheeran',
    '"Can\'t Help Falling in Love" by Elvis Presley',
    '"All of Me" by John Legend',
    '"A Thousand Years" by Christina Perri',
    '"Marry You" by Bruno Mars',
  ];

  const handleSelectGift = (gift: string) => {
    setSelectedGifts((prev) =>
      prev.includes(gift) ? prev.filter((g) => g !== gift) : [...prev, gift]
    );
  };

  const handleSelectSong = (song: string) => {
    setSelectedSongs((prev) =>
      prev.includes(song) ? prev.filter((s) => s !== song) : [...prev, song]
    );
  };

  const handleConfirmGifts = () => {
    console.log("Selected Gifts:", selectedGifts);
    setGiftDialogOpen(false);
  };

  const handleConfirmSongs = () => {
    console.log("Selected Songs:", selectedSongs);
    setSongDialogOpen(false);
  };

  return (
    <Card className='flex-[3] shadow-2xl rounded-3xl overflow-hidden'>
      <CardContent className='p-0' ref={cardRef}>
        <div
          className='relative min-h-[800px] w-full bg-cover bg-center flex items-center justify-center rounded-3xl'
          style={{
            backgroundImage: `url(${backgroundImages[design.backgroundIndex]})`,
          }}>
          <div className='absolute inset-0 bg-black bg-opacity-40 rounded-3xl'></div>
          <AnimatePresence>
            {design.showAnimation && (
              <motion.div
                initial={{ opacity: 0, y: 20 }}
                animate={{ opacity: 1, y: 0 }}
                exit={{ opacity: 0, y: -20 }}
                transition={{ duration: 0.5 }}
                className='relative z-10 text-center p-12 bg-white bg-opacity-20 backdrop-blur-sm rounded-3xl shadow-lg w-[90%] max-w-2xl'
                style={{
                  color: design.textColor,
                  fontSize: `${design.fontSize}px`,
                }}>
                <Heart className='w-24 h-24 mx-auto mb-6 text-pink-500' />
                <h2 className='font-serif text-4xl sm:text-6xl mb-6 font-bold'>
                  {invitationData.coupleNames}
                </h2>
                <p className='mb-8 text-lg sm:text-xl italic'>
                  {invitationData.message}
                </p>
                <div className='flex justify-center items-center mb-6 text-base sm:text-lg'>
                  <Calendar className='w-5 h-5 sm:w-6 sm:h-6 mr-2 sm:mr-3' />
                  <p>
                    {invitationData.date} at {invitationData.time}
                  </p>
                </div>
                <div className='flex justify-center items-center mb-8 text-base sm:text-lg'>
                  <MapPin className='w-5 h-5 sm:w-6 sm:h-6 mr-2 sm:mr-3' />
                  <p>
                    {invitationData.venue}, {invitationData.address}
                  </p>
                </div>

                {/* Gift Registry Dialog */}
                {invitationData.giftLink && (
                  <Dialog
                    open={giftDialogOpen}
                    onOpenChange={setGiftDialogOpen}>
                    <DialogTrigger asChild>
                      <Button
                        variant='outline'
                        className='text-current underline'>
                        View Our Gift Registry
                      </Button>
                    </DialogTrigger>
                    <DialogContent>
                      <DialogHeader>
                        <DialogTitle>Gift Registry</DialogTitle>
                      </DialogHeader>
                      <p>Here are some gift ideas we've selected:</p>
                      <div className='flex flex-col'>
                        {giftOptions.map((gift, index) => (
                          <label key={index} className='flex items-center'>
                            <input
                              type='checkbox'
                              checked={selectedGifts.includes(gift)}
                              onChange={() => handleSelectGift(gift)}
                              className='mr-2'
                            />
                            {gift}
                          </label>
                        ))}
                      </div>
                      <Button
                        onClick={handleConfirmGifts}
                        className='mt-4 w-full bg-green-600 hover:bg-green-700'>
                        Confirm Gifts
                      </Button>
                      <Button asChild>
                        <a
                          href={invitationData.giftLink}
                          target='_blank'
                          rel='noopener noreferrer'
                          className='w-full inline-block text-center'>
                          Go to Gift Registry
                        </a>
                      </Button>
                    </DialogContent>
                  </Dialog>
                )}

                {/* Music Playlist Dialog */}
                {invitationData.musicLink && (
                  <Dialog
                    open={songDialogOpen}
                    onOpenChange={setSongDialogOpen}>
                    <DialogTrigger asChild>
                      <Button
                        variant='outline'
                        className='text-current underline'>
                        Listen to Our Playlist
                      </Button>
                    </DialogTrigger>
                    <DialogContent>
                      <DialogHeader>
                        <DialogTitle>Wedding Playlist</DialogTitle>
                      </DialogHeader>
                      <p>
                        Enjoy our curated playlist and suggest your own songs!
                      </p>
                      <div className='flex flex-col'>
                        {songOptions.map((song, index) => (
                          <label key={index} className='flex items-center'>
                            <input
                              type='checkbox'
                              checked={selectedSongs.includes(song)}
                              onChange={() => handleSelectSong(song)}
                              className='mr-2'
                            />
                            {song}
                          </label>
                        ))}
                      </div>
                      <Button
                        onClick={handleConfirmSongs}
                        className='mt-4 w-full bg-green-600 hover:bg-green-700'>
                        Confirm Songs
                      </Button>
                      <Button asChild>
                        <a
                          href={invitationData.musicLink}
                          target='_blank'
                          rel='noopener noreferrer'
                          className=' w-full inline-block text-center'>
                          Open Playlist
                        </a>
                      </Button>
                    </DialogContent>
                  </Dialog>
                )}

                <p className='text-2xl sm:text-3xl font-bold mt-8'>Countdown</p>
                <p className='text-xl sm:text-2xl font-mono'>{countdown}</p>
              </motion.div>
            )}
          </AnimatePresence>
        </div>
      </CardContent>
    </Card>
  );
}
