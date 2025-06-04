import { Link } from "react-router-dom";
import { Card, CardContent, CardFooter } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Heart } from "lucide-react";

interface InvitationsListProps {
  invitations: { id: number; title: string; image: string }[];
  toggleFavorite: (id: number) => void;
  favorites: number[];
}

export const Invitations = ({
  invitations,
  toggleFavorite,
  favorites,
}: InvitationsListProps) => (
  <div className='grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6'>
    {invitations.map((invitation) => (
      <Card
        key={invitation.id}
        className='overflow-hidden transition-shadow duration-300 hover:shadow-lg'>
        <CardContent className='p-0'>
          <Link to={`/card-single`}>
            <img
              src={invitation.image}
              alt={invitation.title}
              className='w-full h-[380px] object-cover transition-transform duration-300 hover:scale-105'
            />
          </Link>
        </CardContent>
        <CardFooter className='flex justify-between items-center p-4 bg-gray-50'>
          <h4 className='text-lg font-semibold truncate'>{invitation.title}</h4>
          <Button
            variant='ghost'
            size='sm'
            onClick={() => toggleFavorite(invitation.id)}
            className={`p-2 ${
              favorites.includes(invitation.id)
                ? "text-red-500"
                : "text-gray-500"
            }`}>
            <Heart
              size={20}
              fill={favorites.includes(invitation.id) ? "currentColor" : "none"}
            />
            <span className='sr-only'>Toggle favorite</span>
          </Button>
        </CardFooter>
      </Card>
    ))}
  </div>
);
