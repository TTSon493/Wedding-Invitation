import {  Input } from "@/components/ui/input";
import { Textarea } from "../ui/textarea";
import { Button } from "@/components/ui/button";
import { Send, Music } from "lucide-react";

interface GuestInteractionProps {
  guestNote: string;
  setGuestNote: (note: string) => void;
  suggestedSong: string;
  setSuggestedSong: (song: string) => void;
  handleSendNote: () => void;
  handleSuggestSong: () => void;
}

export function GuestInteraction({ guestNote, setGuestNote, suggestedSong, setSuggestedSong, handleSendNote, handleSuggestSong }: GuestInteractionProps) {
  return (
    <>
      <div className="mt-10">
        <Textarea value={guestNote} onChange={(e) => setGuestNote(e.target.value)} placeholder="Write a note to the couple..." />
        <Button onClick={handleSendNote} className="mt-4 w-full py-4">
          <Send className="w-5 h-5 mr-2" /> Send Note
        </Button>
      </div>
      <div className="mt-8">
        <Input value={suggestedSong} onChange={(e) => setSuggestedSong(e.target.value)} placeholder="Suggest a song for the playlist..." />
        <Button onClick={handleSuggestSong} className="mt-4 w-full py-4">
          <Music className="w-5 h-5 mr-2" /> Suggest Song
        </Button>
      </div>
    </>
  );
}
