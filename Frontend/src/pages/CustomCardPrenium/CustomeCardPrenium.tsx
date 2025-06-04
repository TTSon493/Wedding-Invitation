import { useState, useEffect, useRef } from "react";
import InvitationCardPre from "@/components/InvitationCardPre";
import GuestInteraction from "@/components/GuestInteraction";
import CustomizationPanel from "@/components/CustomizationPanel";
import { toast } from "@/hooks/use-toast";

export default function CustomeCardPrenium() {
  const [invitationData, setInvitationData] = useState({
    coupleNames: "Emma & Liam",
    date: "August 15, 2024",
    time: "4:00 PM",
    venue: "Rosewood Garden",
    address: "123 Blossom Street, Springville",
    message: "Join us in celebrating our love story!",
    giftLink: "https://example.com/gift-registry",
    rsvpStatus: "pending",
    musicLink: "https://example.com/wedding-playlist",
  });

  const [design, setDesign] = useState({
    backgroundIndex: 0,
    textColor: "#ffffff",
    fontSize: 16,
    showAnimation: true,
  });

  const [guestView, setGuestView] = useState(false);
  const [countdown, setCountdown] = useState("");
  const [guestNote, setGuestNote] = useState("");
  const [suggestedSong, setSuggestedSong] = useState("");
  const [publicUrl, setPublicUrl] = useState("");
  const cardRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const timer = setInterval(() => {
      const weddingDate = new Date(
        invitationData.date + " " + invitationData.time
      );
      const now = new Date();
      const difference = weddingDate.getTime() - now.getTime();

      if (difference > 0) {
        const days = Math.floor(difference / (1000 * 60 * 60 * 24));
        const hours = Math.floor(
          (difference % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60)
        );
        const minutes = Math.floor(
          (difference % (1000 * 60 * 60)) / (1000 * 60)
        );
        const seconds = Math.floor((difference % (1000 * 60)) / 1000);

        setCountdown(`${days}d ${hours}h ${minutes}m ${seconds}s`);
      } else {
        setCountdown("The wedding day has arrived!");
      }
    }, 1000);

    return () => clearInterval(timer);
  }, [invitationData.date, invitationData.time]);

  const handleInputChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    const { name, value } = e.target;
    setInvitationData((prev) => ({ ...prev, [name]: value }));
  };

  const handleDesignChange = (key: string, value: any) => {
    setDesign((prev) => ({ ...prev, [key]: value }));
  };

  const handlePush = async () => {
    const mockServerUpload = async () => {
      await new Promise((resolve) => setTimeout(resolve, 1500));
      return `https://example.com/wedding-invitation/${Math.random()
        .toString(36)
        .substring(7)}`;
    };

    try {
      const url = await mockServerUpload();
      setPublicUrl(url);
      toast({
        title: "Invitation Pushed",
        description:
          "Your invitation has been successfully pushed to the website.",
      });
    } catch (error) {
      toast({
        title: "Error",
        description:
          "There was an error pushing your invitation. Please try again.",
        variant: "destructive",
      });
    }
  };

  const handleExport = () => {
    const dataStr = `data:text/json;charset=utf-8,${encodeURIComponent(
      JSON.stringify({ invitationData, design })
    )}`;
    const downloadAnchorNode = document.createElement("a");
    downloadAnchorNode.setAttribute("href", dataStr);
    downloadAnchorNode.setAttribute("download", "wedding_invitation.json");
    document.body.appendChild(downloadAnchorNode);
    downloadAnchorNode.click();
    downloadAnchorNode.remove();
  };

  const handleExportHTML = () => {
    if (cardRef.current) {
      const style = document.createElement("style");
      style.textContent = `
        body { font-family: Arial, sans-serif; margin: 0; padding: 0; background-color: #f0f0f0; }
        .invitation-card { max-width: 600px; margin: 20px auto; padding: 20px; background-color: white; box-shadow: 0 0 10px rgba(0,0,0,0.1); border-radius: 10px; }
        .invitation-content { text-align: center; color: ${design.textColor}; font-size: ${design.fontSize}px; }
      `;
      const htmlContent = `
        <!DOCTYPE html>
        <html lang="en">
        <head>
          <meta charset="UTF-8">
          <meta name="viewport" content="width=device-width, initial-scale=1.0">
          <title>Wedding Invitation</title>
          ${style.outerHTML}
        </head>
        <body>
          <div class="invitation-card">
            <div class="invitation-content">
              ${cardRef.current.innerHTML}
            </div>
          </div>
        </body>
        </html>
      `;
      const blob = new Blob([htmlContent], { type: "text/html" });
      const url = URL.createObjectURL(blob);
      const downloadAnchorNode = document.createElement("a");
      downloadAnchorNode.setAttribute("href", url);
      downloadAnchorNode.setAttribute("download", "wedding_invitation.html");
      document.body.appendChild(downloadAnchorNode);
      downloadAnchorNode.click();
      document.body.removeChild(downloadAnchorNode);
      URL.revokeObjectURL(url);
    }
  };

  const handleSendNote = () => {
    toast({
      title: "Note Sent",
      description: "Your note has been sent to the couple.",
    });
    setGuestNote("");
  };

  const handleSuggestSong = () => {
    toast({
      title: "Song Suggested",
      description: "Your song suggestion has been sent to the couple.",
    });
    setSuggestedSong("");
  };

  return (
    <div className='min-h-screen bg-gradient-to-r py-12'>
      <div className='max-w-7xl mx-auto'>
        <h1 className='text-5xl font-bold text-center mb-12'>
          Enchanted Wedding Invitation
        </h1>
        <div className='flex flex-col lg:flex-row gap-12'>
          <InvitationCardPre
            invitationData={invitationData}
            design={design}
            countdown={countdown}
            guestView={guestView}
            cardRef={cardRef}
          />
          <CustomizationPanel
            invitationData={invitationData}
            handleInputChange={handleInputChange}
            design={design}
            handleDesignChange={handleDesignChange}
            guestView={guestView}
            setGuestView={setGuestView}
            handlePush={handlePush}
            handleExport={handleExport}
            handleExportHTML={handleExportHTML}
            publicUrl={publicUrl}
          />
        </div>
        {guestView && (
          <GuestInteraction
            guestNote={guestNote}
            setGuestNote={setGuestNote}
            suggestedSong={suggestedSong}
            setSuggestedSong={setSuggestedSong}
            handleSendNote={handleSendNote}
            handleSuggestSong={handleSuggestSong}
          />
        )}
      </div>
    </div>
  );
}
