import { Button } from "../ui/button";
import { Heart } from "lucide-react";

interface FavoriteButtonProps {
  id: number;
  isFavorite: boolean;
  toggleFavorite: (id: number) => void;
}

export const FavoriteButton = ({
  id,
  isFavorite,
  toggleFavorite,
}: FavoriteButtonProps) => (
  <Button
    variant='ghost'
    size='icon'
    onClick={() => toggleFavorite(id)}
    aria-label={isFavorite ? "Remove from favorites" : "Add to favorites"}>
    <Heart
      className={`h-5 w-5 ${isFavorite ? "fill-red-500 text-red-500" : "text-gray-500"}`}
    />
  </Button>
);
